﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using L5Sharp.Core;
using L5Sharp.Extensions;
using L5Sharp.L5X;
using L5Sharp.Serialization.Components;

namespace L5Sharp.Querying
{
    internal class RungQuery : LogixQuery<Rung>, IRungQuery
    {
        public RungQuery(IEnumerable<XElement> elements)
            : base(elements, new RungSerializer())
        {
        }

        public IRungQuery Flatten()
        {
            var results = new List<XElement>();

            foreach (var element in Elements)
            {
                var text = element.Element(L5XElement.Text.ToString())?.Value.Parse<NeutralText>();

                if (text is null) continue;

                var instructions = text.Instructions();

                foreach (var instruction in instructions)
                {
                    var definition = element.Ancestors(L5XElement.Controller.ToString()).FirstOrDefault()
                        ?.Descendants(L5XElement.AddOnInstructionDefinition.ToString())
                        .FirstOrDefault(e => instruction.Name == e.ComponentName());

                    if (definition is null)
                    {
                        //only add original if not an AOI. AOI is replaced by contained logic
                        results.Add(element);
                        continue;
                    }
                    
                    //Skip first as it is always the aoi tag, which does not have corresponding parameter
                    var arguments = instruction.Operands.Select(o => o.ToString()).Skip(1).ToList();

                    //Only required parameters are part of the instruction signature
                    var parameters = definition.Descendants(L5XElement.Parameter.ToString())
                        .Where(e => bool.Parse(e.Attribute(L5XAttribute.Required.ToString())?.Value!))
                        .Select(p => p.ComponentName());

                    var mapping = arguments.Zip(parameters, (a, p) => new { Argument = a, Parameter = p }).ToList();

                    var rungs = definition.Descendants(L5XElement.Rung.ToString()).ToList();

                    foreach (var rung in rungs)
                    {
                        var value = rung.Element(L5XElement.Text.ToString())?.Value;

                        if (string.IsNullOrEmpty(value))
                            continue;

                        value = mapping.Aggregate(value,
                            (current, pair) => current.Replace(pair.Parameter, pair.Argument));

                        rung.Element(L5XElement.Text.ToString())!.Value = value;

                        results.Add(rung);
                    }
                }
            }

            return new RungQuery(results);
        }

        public IRungQuery InProgram(string programName)
        {
            var results = Elements.Where(e =>
                e.Ancestors(L5XElement.Program.ToString()).FirstOrDefault()?.ComponentName() == programName);

            return new RungQuery(results);
        }

        public IRungQuery InRange(int first, int last)
        {
            var results = Elements.Where(e =>
            {
                var number = int.Parse(e.Attribute(L5XAttribute.Number.ToString())?.Value!);
                return number >= first && number <= last;
            });

            return new RungQuery(results);
        }

        public IRungQuery InRoutine(ComponentName routineName)
        {
            var results = Elements.Where(e =>
                e.Ancestors(L5XElement.Routine.ToString()).FirstOrDefault()?.ComponentName() == (string)routineName);

            return new RungQuery(results);
        }

        public IRungQuery WithDataType(string typeName)
        {
            // we need to find data types of the current tags...
            //1. Get tag names from current collection
            //2. Find tags in document with specified root name
            //3. Need some way to get member names
            //4. Join results on tag name
            throw new NotImplementedException();
        }

        public IRungQuery WithInstruction(string name)
        {
            var results = Elements.Where(e =>
            {
                var text = e.Element(L5XElement.Text.ToString())?.Value;
                return text is not null && string.Equals(text, name);
            });

            return new RungQuery(results);
        }

        public IRungQuery WithTag(TagName tagName)
        {
            var results = Elements.Where(e =>
            {
                var text = e.Attribute(L5XElement.Text.ToString())?.Value;
                return text is not null && text.Contains(tagName);
            });

            return new RungQuery(results);
        }
    }
}