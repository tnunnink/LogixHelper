﻿using Ardalis.SmartEnum;

namespace L5Sharp.Enumerations
{
    public class DataTypeClass : SmartEnum<DataTypeClass>
    {
        private DataTypeClass(string name, int value) : base(name, value)
        {
        }

        public static readonly DataTypeClass User = new DataTypeClass("User", 0);
        public static readonly DataTypeClass Predefined = new DataTypeClass("ProductDefined", 1); 
        public static readonly DataTypeClass Io = new DataTypeClass("IO", 2);
    }
}