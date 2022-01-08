﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Ardalis.SmartEnum;
using L5Sharp.Extensions;
using L5Sharp.Types;

namespace L5Sharp.Enums
{
    /// <summary>
    /// Represents a number base for a given value type or atomic type.
    /// </summary>
    public abstract class Radix : SmartEnum<Radix, string>
    {
        private static readonly Dictionary<string, Func<string, bool>> Identifiers = new()
        {
            { nameof(Binary), s => s.HasBinaryFormat() },
            { nameof(Octal), s => s.HasOctalFormat() },
            { nameof(Decimal), s => s.HasDecimalFormat() },
            { nameof(Hex), s => s.HasHexFormat() },
            { nameof(Float), s => s.HasFloatFormat() },
            { nameof(Exponential), s => s.HasExponentialFormat() },
            { nameof(Ascii), s => s.HasAsciiFormat() },
            { nameof(DateTime), s => s.HasDateTimeFormat() },
            { nameof(DateTimeNs), s => s.HasDateTimeNsFormat() }
        };

        private Radix(string name, string value) : base(name, value)
        {
        }

        /// <summary>
        /// Represents a Null radix, or absence of a Radix value.
        /// </summary>
        /// <remarks>
        /// Only <see cref="IAtomicType"/> types have non-null Radix. <see cref="IComplexType"/> types all have null Radix.
        /// </remarks>
        public static readonly Radix Null = new NullRadix();

        /// <summary>
        /// Represents a Binary number base format.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Binary Radix format starts with the specifier string '2#'.
        /// Each byte is separated by a '_' character.
        /// The string value is padded based on the size of the data type.
        /// </para>
        /// <para>
        /// Valid Types: <see cref="Bool"/>, <see cref="Sint"/>, <see cref="Int"/>, <see cref="Dint"/>, <see cref="Lint"/>.
        /// </para> 
        /// </remarks>
        /// <example>
        /// Int with value 5 would be '2#0000_0101'.
        /// </example>
        public static readonly Radix Binary = new BinaryRadix();

        /// <summary>
        /// Represents a Octal number base format. 
        /// </summary>
        public static readonly Radix Octal = new OctalRadix();

        /// <summary>
        /// Represents a Decimal number base format.
        /// </summary>
        public static readonly Radix Decimal = new DecimalRadix();

        /// <summary>
        /// Represents a Hexadecimal number base format.
        /// </summary>
        public static readonly Radix Hex = new HexRadix();

        /// <summary>
        /// Represents a Exponential number base format.
        /// </summary>
        public static readonly Radix Exponential = new ExponentialRadix();

        /// <summary>
        /// Represents a Float number base format.
        /// </summary>
        public static readonly Radix Float = new FloatRadix();

        /// <summary>
        /// Represents a Ascii number base format.
        /// </summary>
        public static readonly Radix Ascii = new AsciiRadix();

        /// <summary>
        /// Represents a DateTime number base format.
        /// </summary>
        public static readonly Radix DateTime = new DateTimeRadix();

        /// <summary>
        /// Represents a DateTimeNs number base format.
        /// </summary>
        public static readonly Radix DateTimeNs = new DateTimeNsRadix();

        /// <summary>
        /// Gets the default Radix type for the provided data type instance.
        /// </summary>
        /// <param name="dataType">The data type to determine the default Radix for.</param>
        /// <returns>
        /// <see cref="Null"/> for all non atomic types.
        /// <see cref="Float"/> for <see cref="Real"/> types.
        /// <see cref="Decimal"/> for all other atomic types.
        /// </returns>
        public static Radix Default(IDataType dataType) => Default(dataType.GetType());

        /// <summary>
        /// Gets the default Radix type for the provided Type.
        /// </summary>
        /// <param name="type">The type to determine the default Radix for.</param>
        /// <returns>
        /// /// <see cref="Null"/> for all non atomic types.
        /// <see cref="Float"/> for <see cref="Real"/> types.
        /// <see cref="Decimal"/> for all other atomic types.
        /// </returns>
        public static Radix Default(Type type)
        {
            if (typeof(IArrayType<IDataType>).IsAssignableFrom(type))
                type = type.GetGenericArguments()[0];
            
            if (!typeof(IAtomicType).IsAssignableFrom(type))
                return Null;

            return type == typeof(Real) ? Float : Decimal;
        }

        /// <summary>
        /// Determines if the current Radix value supports the provided data type instance.
        /// </summary>
        /// <param name="dataType">The data type instance to evaluate.</param>
        /// <returns>true if the current radix value is valid for the given data type instance. Otherwise, false.</returns>
        public bool SupportsType(IDataType dataType) => SupportsType(dataType.GetType());

        /// <summary>
        /// Determines if the current Radix value supports the provided Type.
        /// </summary>
        /// <param name="type">The type to be evaluated for support.</param>
        /// <returns>
        /// true if the current Radix value is supported for the given Type. Otherwise, false.
        /// </returns>
        public bool SupportsType(Type type)
        {
            if (typeof(IArrayType<IDataType>).IsAssignableFrom(type))
                type = type.GetGenericArguments()[0];
            
            if (!typeof(IAtomicType).IsAssignableFrom(type))
                return Equals(Null);

            if (type == typeof(Bool))
                return Equals(Binary) || Equals(Octal) || Equals(Decimal) || Equals(Hex);

            if (type == typeof(Lint))
                return Equals(Binary) || Equals(Octal) || Equals(Decimal) || Equals(Hex) || Equals(Ascii) ||
                       Equals(DateTime) || Equals(DateTimeNs);

            if (type == typeof(Real))
                return Equals(Float) || Equals(Exponential);

            return Equals(Binary) || Equals(Octal) || Equals(Decimal) || Equals(Hex) || Equals(Ascii);
        }

        /// <summary>
        /// Determines a Radix type based on the provided string value.
        /// </summary>
        /// <remarks>
        /// For instances where the radix is not known, it can be inferred from the identifier of the string input.
        /// </remarks>
        /// <param name="value">The string input to infer the Radix type for.</param>
        /// <returns>A Radix type representing the format of the string input.</returns>
        public static Radix Infer(string value)
        {
            var radix = Identifiers.FirstOrDefault(i => i.Value.Invoke(value)).Key;

            return radix is not null ? FromName(radix) : Null;
        }

        /// <summary>
        /// Parses a string input to an object value based on the format of the input value.
        /// </summary>
        /// <remarks>
        /// This method determines the radix based on patterns in the input string. For example, if the string input
        /// starts with the specifier '2#', this method will forward the call to <see cref="Parse"/> for the Binary Radix
        /// and return the result. If no radix can be determined from the input string, the call is forwarded to the
        /// <see cref="Null"/> radix, which simply returns the input string.
        /// </remarks>
        /// <param name="input">The string value to parse.</param>
        /// <returns>
        /// An object representing the value of the parsed string input.
        /// If no radix format can be determined from the input, returns the input string.
        /// </returns>
        public static object ParseValue(string input)
        {
            var parser = GetParser(input);

            var value = parser(input);

            if (value is null or string)
                throw new ArgumentException(
                    $"Could not parse string '{input}'. Verify that the string is an accepted Radix format.");

            return value;
        }

        /// <summary>
        /// Parsed a string input and returns the value as an <see cref="IAtomicType"/> value type.
        /// </summary>
        /// <remarks>
        /// This method is similar to <see cref="ParseValue"/>, except it will return the parsed input as the
        /// atomic value type that is specified by the generic parameter.
        /// </remarks>
        /// <param name="input">The string value to parse.</param>
        /// <typeparam name="TAtomic">The <see cref="IAtomicType"/> type to return.</typeparam>
        /// <returns>
        /// An IAtomic value type instance representing the value of the parsed string input.
        /// </returns>
        public static TAtomic ParseValue<TAtomic>(string input) where TAtomic : IAtomicType, new()
        {
            var parser = GetParser(input);

            var value = parser(input);

            if (value is null or string)
                throw new ArgumentException(
                    $"Could not parse string '{input}' to {typeof(TAtomic)}. Verify that the string is an accepted Radix format.");

            var atomic = new TAtomic();

            atomic.SetValue(value);

            return atomic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool TryParseValue(string input, out object? result)
        {
            try
            {
                var parser = GetParser(input);

                var value = parser(input);

                if (value is null or string)
                    throw new ArgumentException(
                        $"Could not parse string '{input}'. Verify that the string is an accepted Radix format.");

                result = value;
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Converts an atomic value to the current radix base value. 
        /// </summary>
        /// <param name="atomic">The current atomic type to convert.</param>
        /// <returns>
        /// A string that represents the value of the atomic type in the current radix base number style.
        /// </returns>
        public abstract string Convert(IAtomicType atomic);

        /// <summary>
        /// Parses a string input of a given Radix formatted value into an object value. 
        /// </summary>
        /// <param name="input">The string value to parse.</param>
        /// <returns>An object representing the value of the formatted string.</returns>
        public abstract object? Parse(string input);

        /// <summary>
        /// Converts the atomic value into the specified base number type.
        /// </summary>
        /// <param name="atomic">The atomic type to convert.</param>
        /// <param name="baseNumber">The base number to convert to.</param>
        /// <returns>
        /// A string representing the value of the atomic in the specified base number.
        /// If not convertable, returns an empty string.
        /// </returns>
        private static string ChangeBase(IAtomicType atomic, int baseNumber)
        {
            return atomic switch
            {
                Bool b => b ? "1" : "0",
                Sint s => System.Convert.ToString(s.Value, baseNumber),
                Int i => System.Convert.ToString(i.Value, baseNumber),
                Dint d => System.Convert.ToString(d.Value, baseNumber),
                Lint l => System.Convert.ToString(l.Value, baseNumber),
                _ => string.Empty
            };
        }

        /// <summary>
        /// Gets a parse function based on the provided string input.
        /// </summary>
        /// <param name="value">The string input value to determine a parse function for.</param>
        /// <returns>
        /// A func delegate that represents the parse function for the given string input.
        /// </returns>
        private static Func<string, object?> GetParser(string value)
        {
            var radix = Identifiers.FirstOrDefault(i => i.Value.Invoke(value)).Key;

            return radix is not null ? FromName(radix).Parse : Null.Parse;
        }

        private void ValidateType(IAtomicType atomic)
        {
            if (atomic == null)
                throw new ArgumentNullException(nameof(atomic));

            if (!SupportsType(atomic))
                throw new NotSupportedException($"{atomic.GetType()} is not supported by {Name} Radix.");
        }

        private void ValidateFormat(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            if (!Identifiers[Name].Invoke(input))
                throw new FormatException($"Input '{input}' does not have expected {Name} format.");
        }

        private class NullRadix : Radix
        {
            public NullRadix() : base("NullType", "NullType")
            {
            }

            public override string Convert(IAtomicType atomic)
            {
                return string.Empty;
            }

            public override object? Parse(string input)
            {
                return null;
            }
        }

        private class BinaryRadix : Radix
        {
            private const int BaseNumber = 2;
            private const string ByteSeparator = "_";
            private const string Specifier = "2#";
            private const string Pattern = @"(?<=\d)(?=(\d\d\d\d)+(?!\d))";

            public BinaryRadix() : base(nameof(Binary), nameof(Binary))
            {
            }

            public override string Convert(IAtomicType atomic)
            {
                ValidateType(atomic);

                var str = ChangeBase(atomic, BaseNumber);

                str = atomic switch
                {
                    Bool => str.PadLeft(0, '0'),
                    Sint => str.PadLeft(8, '0'),
                    Int => str.PadLeft(16, '0'),
                    Dint => str.PadLeft(32, '0'),
                    Lint => str.PadLeft(64, '0'),
                    _ => throw new NotSupportedException($"{atomic.GetType()} not supported for {Name} Radix.")
                };

                str = Regex.Replace(str, Pattern, ByteSeparator, RegexOptions.Compiled);

                return $"{Specifier}{str}";
            }

            public override object Parse(string input)
            {
                ValidateFormat(input);

                var value = input.Replace(Specifier, string.Empty).Replace(ByteSeparator, string.Empty);

                return value.Length switch
                {
                    1 => value == "1",
                    > 1 and <= 8 => System.Convert.ToByte(value, BaseNumber),
                    > 8 and <= 16 => System.Convert.ToInt16(value, BaseNumber),
                    > 16 and <= 32 => System.Convert.ToInt32(value, BaseNumber),
                    > 32 and <= 64 => System.Convert.ToInt64(value, BaseNumber),
                    _ => throw new ArgumentOutOfRangeException(nameof(value.Length),
                        $"The value {value.Length} is out of range for {Binary} Radix.")
                };
            }
        }

        private class OctalRadix : Radix
        {
            private const int BaseNumber = 8;
            private const string ByteSeparator = "_";
            private const string Specifier = "8#";
            private const string Pattern = @"(?<=\d)(?=(\d\d\d)+(?!\d))";

            public OctalRadix() : base(nameof(Octal), nameof(Octal))
            {
            }

            public override string Convert(IAtomicType atomic)
            {
                ValidateType(atomic);

                var str = ChangeBase(atomic, BaseNumber);

                str = atomic switch
                {
                    Bool => str.PadLeft(0, '0'),
                    Sint => str.PadLeft(3, '0'),
                    Int => str.PadLeft(6, '0'),
                    Dint => str.PadLeft(11, '0'),
                    Lint => str.PadLeft(22, '0'),
                    _ => throw new NotSupportedException($"{atomic.GetType()} not supported for {Name} Radix.")
                };

                str = Regex.Replace(str, Pattern, ByteSeparator, RegexOptions.Compiled);

                return $"{Specifier}{str}";
            }

            public override object Parse(string input)
            {
                ValidateFormat(input);

                var value = input.Replace(Specifier, string.Empty).Replace(ByteSeparator, string.Empty);

                return value.Length switch
                {
                    1 => value == "1",
                    > 1 and <= 3 => System.Convert.ToByte(value, BaseNumber),
                    > 3 and <= 6 => System.Convert.ToInt16(value, BaseNumber),
                    > 6 and <= 11 => System.Convert.ToInt32(value, BaseNumber),
                    > 11 and <= 22 => System.Convert.ToInt64(value, BaseNumber),
                    _ => throw new ArgumentOutOfRangeException(nameof(value.Length),
                        $"The value {value.Length} is out of range for {Octal} Radix.")
                };
            }
        }

        private class DecimalRadix : Radix
        {
            public DecimalRadix() : base(nameof(Decimal), nameof(Decimal))
            {
            }

            public override string Convert(IAtomicType atomic)
            {
                ValidateType(atomic);

                return ChangeBase(atomic, 10);
            }

            public override object Parse(string input)
            {
                ValidateFormat(input);

                if (byte.TryParse(input, out var byteValue))
                    return byteValue;

                if (short.TryParse(input, out var shortValue))
                    return shortValue;

                if (int.TryParse(input, out var intValue))
                    return intValue;

                if (long.TryParse(input, out var longValue))
                    return longValue;

                throw new ArgumentException($"Input '{input}' not valid for {Decimal} Radix.");
            }
        }

        private class HexRadix : Radix
        {
            private const int BaseNumber = 16;
            private const string ByteSeparator = "_";
            private const string Specifier = "16#";
            private const string Pattern = @"(?<=\w)(?=(\w\w\w\w)+(?!\w))";

            public HexRadix() : base(nameof(Hex), nameof(Hex))
            {
            }

            public override string Convert(IAtomicType atomic)
            {
                ValidateType(atomic);

                var str = ChangeBase(atomic, BaseNumber);

                str = atomic switch
                {
                    Bool => str.PadLeft(0, '0'),
                    Sint => str.PadLeft(2, '0'),
                    Int => str.PadLeft(4, '0'),
                    Dint => str.PadLeft(8, '0'),
                    Lint => str.PadLeft(16, '0'),
                    _ => throw new NotSupportedException($"{atomic.GetType()} not supported for {Name} Radix.")
                };

                str = Regex.Replace(str, Pattern, ByteSeparator, RegexOptions.Compiled);

                return $"{Specifier}{str}";
            }

            public override object Parse(string input)
            {
                ValidateFormat(input);

                var value = input.Replace(Specifier, string.Empty).Replace(ByteSeparator, string.Empty);

                return value.Length switch
                {
                    1 => value == "1",
                    2 => System.Convert.ToByte(value, BaseNumber),
                    4 => System.Convert.ToInt16(value, BaseNumber),
                    8 => System.Convert.ToInt32(value, BaseNumber),
                    16 => System.Convert.ToInt64(value, BaseNumber),
                    _ => throw new ArgumentOutOfRangeException(nameof(value.Length),
                        $"The value {value.Length} is out of range for {Hex} Radix.")
                };
            }
        }

        private class FloatRadix : Radix
        {
            public FloatRadix() : base(nameof(Float), nameof(Float))
            {
            }

            public override string Convert(IAtomicType atomic)
            {
                ValidateType(atomic);

                var value = (float)atomic.Value;

                return value.ToString("0.0######", CultureInfo.InvariantCulture);
            }

            public override object Parse(string input)
            {
                ValidateFormat(input);

                if (float.TryParse(input, out var result))
                    return result;

                throw new ArgumentException($"Input '{input}' not a valid floating point number.");
            }
        }

        private class ExponentialRadix : Radix
        {
            public ExponentialRadix() : base(nameof(Exponential), nameof(Exponential))
            {
            }

            public override string Convert(IAtomicType atomic)
            {
                ValidateType(atomic);

                var value = (float)atomic.Value;

                return value.ToString("e8", CultureInfo.InvariantCulture);
            }

            public override object Parse(string input)
            {
                ValidateFormat(input);

                if (!float.TryParse(input, out var result))
                    throw new ArgumentException($"Input '{input}' not valid for {Exponential} Radix.");

                return result;
            }
        }

        private class AsciiRadix : Radix
        {
            private const int BaseNumber = 16;
            private const string ByteSeparator = "$";

            public AsciiRadix() : base(nameof(Ascii).ToUpper(), nameof(Ascii).ToUpper())
            {
            }

            public override string Convert(IAtomicType atomic)
            {
                ValidateType(atomic);

                var str = ChangeBase(atomic, BaseNumber);

                str = atomic switch
                {
                    Sint _ => str.PadLeft(2, '0'),
                    Int _ => str.PadLeft(4, '0'),
                    Dint _ => str.PadLeft(8, '0'),
                    Lint _ => str.PadLeft(16, '0'),
                    _ => throw new NotSupportedException($"{atomic.GetType()} not supported for {Ascii} Radix.")
                };

                return Regex.Replace(str, @"(?=(\w\w)+(?!\w))", ByteSeparator);
            }

            public override object Parse(string input)
            {
                ValidateFormat(input);

                var value = input.Replace(ByteSeparator, string.Empty);

                return value.Length switch
                {
                    2 => System.Convert.ToByte(value, BaseNumber),
                    4 => System.Convert.ToInt16(value, BaseNumber),
                    8 => System.Convert.ToInt32(value, BaseNumber),
                    16 => System.Convert.ToInt64(value, BaseNumber),
                    _ => throw new ArgumentOutOfRangeException(nameof(value.Length),
                        $"The value {value.Length} is out of range for {Hex} Radix.")
                };
            }
        }

        private class DateTimeRadix : Radix
        {
            private const string Specifier = "DT#";

            public DateTimeRadix() : base(nameof(DateTime), nameof(DateTime))
            {
            }

            public override string Convert(IAtomicType atomic)
            {
                ValidateType(atomic);

                //Calculate local time from provided long value.
                var seconds = (long)atomic.Value / 1000000;
                var dateTime = System.DateTime.UnixEpoch.AddSeconds(seconds).ToLocalTime();
                var offset = System.DateTime.SpecifyKind(dateTime, DateTimeKind.Local);

                // ReSharper disable once StringLiteralTypo
                var formatted = offset.ToString("yyyy-MM-dd-HH:mm:ss.ffffff(UTCzzz)");

                return $"{Specifier}{formatted}";
            }

            public override object Parse(string input)
            {
                ValidateFormat(input);

                var value = input.Replace(Specifier, string.Empty);

                return System.DateTime.ParseExact(value, "yyyy-MM-dd-HH:mm:ss.ffffff(UTCzzz)",
                    CultureInfo.InvariantCulture);
            }
        }


        private class DateTimeNsRadix : Radix
        {
            private const string Specifier = "LDT#";

            public DateTimeNsRadix() : base(nameof(DateTimeNs), nameof(DateTimeNs))
            {
            }

            public override string Convert(IAtomicType atomic)
            {
                ValidateType(atomic);

                //Calculate local time from provided long value.
                var seconds = (long)atomic.Value / 100;
                var dateTime = System.DateTime.UnixEpoch.AddTicks(seconds).ToLocalTime();
                var offset = System.DateTime.SpecifyKind(dateTime, DateTimeKind.Local);

                // ReSharper disable once StringLiteralTypo (this is on UTC zzz)
                var formatted = offset.ToString("yyyy-MM-dd-HH:mm:ss.fffffff(UTCzzz)");

                return $"{Specifier}{formatted}";
            }

            public override object Parse(string input)
            {
                ValidateFormat(input);

                var value = input.Replace(Specifier, string.Empty);

                return System.DateTime.ParseExact(value, "yyyy-MM-dd-HH:mm:ss.ffffff(UTCzzz)",
                    CultureInfo.InvariantCulture);
            }
        }
    }
}