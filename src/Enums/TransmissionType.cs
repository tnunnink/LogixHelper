﻿using Ardalis.SmartEnum;

namespace L5Sharp.Enums
{
    /// <summary>
    /// Represents an enumer
    /// </summary>
    public class TransmissionType : SmartEnum<TransmissionType>
    {
        public TransmissionType(string name, int value) : base(name, value)
        {
        }

        public static readonly TransmissionType Null = new TransmissionType("Null", 0);
        public static readonly TransmissionType Multicast = new TransmissionType("Multicast", 1);
        public static readonly TransmissionType Unicast = new TransmissionType("Unicast", 2);
    }
}