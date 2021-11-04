﻿using L5Sharp.Core;
using L5Sharp.Enums;

namespace L5Sharp.Types
{
    public sealed class Dint : Atomic
    {
        public Dint() : base(nameof(Dint).ToUpper())
        {
        }

        public override object DefaultValue => default(int);

        public override bool IsValidValue(object value)
        {
            if (value is string)
                value = ParseValue(value.ToString());
            
            return value is int;
        }

        public override object ParseValue(string value)
        {
            if (int.TryParse(value, out var result))
                return result;
            return null;
        }
    }
}