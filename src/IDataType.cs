﻿using L5Sharp.Enums;

namespace L5Sharp
{
    public interface IDataType : ILogixComponent
    {
        public DataTypeFamily Family { get; }
        public DataTypeClass Class { get; }
        public TagDataFormat DataFormat { get; }
    }
}