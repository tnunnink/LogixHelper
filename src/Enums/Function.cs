﻿namespace L5Sharp.Enums
{
    /// <summary>
    /// An enumeration of known Logix arithmetic functions.
    /// </summary>
    public class Function : LogixEnum<Function, string>
    {
        private Function(string name, string value) : base(name, value)
        {
        }

        /// <summary>
        /// Determines if the provided string input key is a known logix function.
        /// </summary>
        /// <param name="key">The string to test.</param>
        /// <returns>true if the provided key is a valid function value; otherwise false.</returns>
        public static bool IsValid(string key) => TryFromValue(key, out _);

        /// <summary>
        /// Represents the Absolute Value Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function ABS = new("Absolute Value", nameof(ABS));
        
        /// <summary>
        /// Represents the Arc Cosine Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function ACS = new("Arc Cosine", nameof(ACS));
        
        /// <summary>
        /// Represents the Arc Sine Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function ASN = new("Arc Sine", nameof(ASN));
        
        /// <summary>
        /// Represents the Arc Tangent Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function ATN = new("Arc Tangent", nameof(ATN));
        
        /// <summary>
        /// Represents the Cosine Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function COS = new("Cosine", nameof(COS));
        
        /// <summary>
        /// Represents the Radians To Degrees Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function DEG = new("Radians To Degrees", nameof(DEG));
        
        /// <summary>
        /// Represents the Natural Log Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function LN = new("Natural Log", nameof(LN));
        
        /// <summary>
        /// Represents the Log Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function LOG = new("Log", nameof(LOG));
        
        /// <summary>
        /// Represents the Radians Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function RAD = new("Radians", nameof(RAD));
        
        /// <summary>
        /// Represents the Sine Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function SIN = new("Sine", nameof(SIN));
        
        /// <summary>
        /// Represents the Square Root Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function SQRT = new("Square Root", nameof(SQRT));
        
        /// <summary>
        /// Represents the Tangent Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function TAN = new("Tangent", nameof(TAN));
        
        /// <summary>
        /// Represents the Truncate Arithmetic Logix <see cref="Function"/>.
        /// </summary>
        public static readonly Function TRUNC = new("Truncate", nameof(TRUNC));
    }
}