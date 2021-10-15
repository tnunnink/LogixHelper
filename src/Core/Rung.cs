﻿using L5Sharp.Enums;

namespace L5Sharp.Core
{
    public class Rung
    {
        public int Number { get; set; }
        public RungType Type { get; set; }
        public string Comment { get; set; }
        public string Text { get; set; }
    }
}