﻿using System.Xml.Linq;
using L5Sharp.Abstractions;

namespace L5Sharp.Core
{
    public class Instruction : IInstruction
    {
        public string Name { get; }
        public XElement Serialize()
        {
            throw new System.NotImplementedException();
        }
    }
}