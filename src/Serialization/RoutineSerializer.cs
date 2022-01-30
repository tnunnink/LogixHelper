﻿using System;
using System.Xml.Linq;
using L5Sharp.Core;
using L5Sharp.Enums;
using L5Sharp.Extensions;
using L5Sharp.Helpers;

namespace L5Sharp.Serialization
{
    /// <summary>
    /// Provides serialization of a <see cref="IRoutine{TContent}"/> as represented in the L5X format. 
    /// </summary>
    internal class RoutineSerializer : IXSerializer<IRoutine<ILogixContent>>
    {
        private readonly LogixContext _context;
        private static readonly XName ElementName = LogixNames.Routine;

        public RoutineSerializer(LogixContext context)
        {
            _context = context;
        }
        
        /// <inheritdoc />
        public XElement Serialize(IRoutine<ILogixContent> component)
        {
            if (component is null)
                throw new ArgumentNullException(nameof(component));

            var element = new XElement(ElementName);
            
            element.AddAttribute(component, c => c.Name);
            element.AddElement(component, c => c.Description);
            element.AddAttribute(component, c => c.Type);
            element.Add(_context.Serializer.Serialize(component.Content));

            return element;
        }

        /// <inheritdoc />
        public IRoutine<ILogixContent> Deserialize(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (element.Name != ElementName)
                throw new ArgumentException($"Element name '{element.Name}' invalid. Expecting '{ElementName}'");

            var name = element.GetComponentName();
            var description = element.GetComponentDescription();
            var type = element.GetAttribute<IRoutine<ILogixContent>, RoutineType>(r => r.Type);
            
            //todo content ??
            /*var content = element.Element(LogixNames.RllContent) is not null 
                ? element.Element(LogixNames.RllContent).Deserialize<IRllContent>() : null;*/

            return new Routine<ILogixContent>(name, type, description);
        }
    }
}