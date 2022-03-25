﻿using System;
using System.ComponentModel;
using AutoFixture;
using FluentAssertions;
using L5Sharp.Internal.Tests.Specimens;
using L5Sharp.Types;
using NUnit.Framework;

namespace L5Sharp.Internal.Tests.Converters
{
    [TestFixture]
    public class USintConverterTests
    {
        private Fixture _fixture;
        private TypeConverter _converter;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new BoolGenerator());
            _fixture.Customizations.Add(new SintGenerator());
            _fixture.Customizations.Add(new USintGenerator());
            _fixture.Customizations.Add(new IntGenerator());
            _fixture.Customizations.Add(new UIntGenerator());
            _fixture.Customizations.Add(new DintGenerator());
            _fixture.Customizations.Add(new UDintGenerator());
            _fixture.Customizations.Add(new LintGenerator());
            _fixture.Customizations.Add(new ULintGenerator());

            _converter = TypeDescriptor.GetConverter(typeof(USINT));
        }
        
        [Test]
        public void CanConvertFrom_bool_ShouldBeFalse()
        {
            var value = _fixture.Create<bool>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_sbyte_ShouldBeFalse()
        {
            var value = _fixture.Create<sbyte>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }

        [Test]
        public void CanConvertFrom_byte_ShouldBeTrue()
        {
            var value = _fixture.Create<byte>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertFrom_short_ShouldBeFalse()
        {
            var value = _fixture.Create<short>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_ushort_ShouldBeFalse()
        {
            var value = _fixture.Create<ushort>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_int_ShouldBeFalse()
        {
            var value = _fixture.Create<int>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_uint_ShouldBeFalse()
        {
            var value = _fixture.Create<uint>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_long_ShouldBeFalse()
        {
            var value = _fixture.Create<long>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_ulong_ShouldBeFalse()
        {
            var value = _fixture.Create<ulong>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_Bool_ShouldBeFalse()
        {
            var value = _fixture.Create<BOOL>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }

        [Test]
        public void CanConvertFrom_Sint_ShouldBeFalse()
        {
            var value = _fixture.Create<SINT>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_USint_ShouldBeTure()
        {
            var value = _fixture.Create<USINT>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertFrom_Int_ShouldBeFalse()
        {
            var value = _fixture.Create<INT>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_UInt_ShouldBeFalse()
        {
            var value = _fixture.Create<UINT>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_Dint_ShouldBeFalse()
        {
            var value = _fixture.Create<DINT>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_UDint_ShouldBeFalse()
        {
            var value = _fixture.Create<UDINT>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_Lint_ShouldBeFalse()
        {
            var value = _fixture.Create<LINT>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_ULint_ShouldBeFalse()
        {
            var value = _fixture.Create<ULINT>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }
        
        [Test]
        public void CanConvertFrom_Real_ShouldBeFalse()
        {
            var value = _fixture.Create<REAL>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeFalse();
        }

        [Test]
        public void CanConvertFrom_string_ShouldBeTrue()
        {
            var value = _fixture.Create<string>();

            var result = _converter.CanConvertFrom(value.GetType());

            result.Should().BeTrue();
        }

        [Test]
        public void ConvertFrom_bool_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<bool>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }
        
        [Test]
        public void ConvertFrom_sbyte_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<sbyte>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }

        [Test]
        public void ConvertFrom_byte_ShouldBeExpected()
        {
            var value = _fixture.Create<byte>();

            var result = (USINT)_converter.ConvertFrom(value);

            result.Should().Be(result);
        }
        
        [Test]
        public void ConvertFrom_short_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<short>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }
        
        [Test]
        public void ConvertFrom_ushort_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<ushort>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }
        
        [Test]
        public void ConvertFrom_int_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<int>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }
        
        [Test]
        public void ConvertFrom_uint_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<uint>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }
        
        [Test]
        public void ConvertFrom_long_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<long>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }
        
        [Test]
        public void ConvertFrom_ulong_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<ulong>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }

        [Test]
        public void ConvertFrom_float_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<float>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }

        [Test]
        public void ConvertFrom_Sint_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<SINT>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }

        [Test]
        public void ConvertFrom_USint_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (USINT)_converter.ConvertFrom(value);

            result.Should().Be(result);
        }

        [Test]
        public void ConvertFrom_Int_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<INT>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }

        [Test]
        public void ConvertFrom_UInt_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<UINT>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }

        [Test]
        public void ConvertFrom_Dint_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<DINT>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }

        [Test]
        public void ConvertFrom_UDint_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<UDINT>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }

        [Test]
        public void ConvertFrom_Lint_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<LINT>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }

        [Test]
        public void ConvertFrom_ULint_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<ULINT>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }
        
        [Test]
        public void ConvertFrom_Real_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<REAL>();

            FluentActions.Invoking(() => _converter.ConvertFrom(value)).Should().Throw<NotSupportedException>();
        }
        
        [Test]
        public void ConvertFrom_string_ShouldBeExpected()
        {
            var value = _fixture.Create<byte>().ToString();

            var result = (USINT)_converter.ConvertFrom(value);

            result.Should().Be(result);
        }
        
        [Test]
        public void CanConvertTo_sbyte_ShouldBeFalse()
        {
            var value = _fixture.Create<sbyte>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeFalse();
        }

        [Test]
        public void CanConvertTo_byte_ShouldBeTrue()
        {
            var value = _fixture.Create<byte>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_short_ShouldBeTrue()
        {
            var value = _fixture.Create<short>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_ushort_ShouldBeTrue()
        {
            var value = _fixture.Create<ushort>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_int_ShouldBeTrue()
        {
            var value = _fixture.Create<int>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_uint_ShouldBeTrue()
        {
            var value = _fixture.Create<uint>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_long_ShouldBeTrue()
        {
            var value = _fixture.Create<long>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_ulong_ShouldBeTrue()
        {
            var value = _fixture.Create<ulong>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_float_ShouldBeTrue()
        {
            var value = _fixture.Create<float>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_Sint_ShouldBeFalse()
        {
            var value = _fixture.Create<SINT>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeFalse();
        }

        [Test]
        public void CanConvertTo_USint_ShouldBeTrue()
        {
            var value = _fixture.Create<USINT>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_Int_ShouldBeTrue()
        {
            var value = _fixture.Create<INT>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }

        [Test]
        public void CanConvertTo_UInt_ShouldBeTrue()
        {
            var value = _fixture.Create<UINT>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }

        [Test]
        public void CanConvertTo_Dint_ShouldBeTrue()
        {
            var value = _fixture.Create<DINT>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_UDint_ShouldBeTrue()
        {
            var value = _fixture.Create<UDINT>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_Lint_ShouldBeTrue()
        {
            var value = _fixture.Create<LINT>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_ULint_ShouldBeTrue()
        {
            var value = _fixture.Create<ULINT>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }
        
        [Test]
        public void CanConvertTo_Real_ShouldBeTrue()
        {
            var value = _fixture.Create<REAL>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }

        [Test]
        public void CanConvertTo_string_ShouldBeTrue()
        {
            var value = _fixture.Create<string>();

            var result = _converter.CanConvertTo(value.GetType());

            result.Should().BeTrue();
        }

        [Test]
        public void ConvertTo_Null_ShouldThrowInvalidOperationException()
        {
            FluentActions.Invoking(() => _converter.ConvertTo(null!, typeof(bool))!).Should()
                .Throw<InvalidOperationException>();
        }

        [Test]
        public void ConvertTo_bool_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<USINT>();
            
            FluentActions.Invoking(() => _converter.ConvertTo(value, typeof(bool))!)
                .Should().Throw<NotSupportedException>();
        }
        
        [Test]
        public void ConvertTo_sbyte_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<USINT>();
            
            FluentActions.Invoking(() => _converter.ConvertTo(value, typeof(sbyte))!)
                .Should().Throw<NotSupportedException>();
        }

        [Test]
        public void ConvertTo_byte_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (byte)_converter.ConvertTo(value, typeof(byte))!;

            result.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_short_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (short)_converter.ConvertTo(value, typeof(short))!;

            result.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_ushort_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (ushort)_converter.ConvertTo(value, typeof(ushort))!;

            result.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_int_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (int)_converter.ConvertTo(value, typeof(int))!;

            result.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_uint_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (uint)_converter.ConvertTo(value, typeof(uint))!;

            result.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_long_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (long)_converter.ConvertTo(value, typeof(long))!;

            result.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_ulong_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (ulong)_converter.ConvertTo(value, typeof(ulong))!;

            result.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_float_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (float)_converter.ConvertTo(value, typeof(float))!;

            result.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_Bool_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<USINT>();
            
            FluentActions.Invoking(() => _converter.ConvertTo(value, typeof(BOOL))!)
                .Should().Throw<NotSupportedException>();
        }
        
        [Test]
        public void ConvertTo_Sint_ShouldThrowNotSupportedException()
        {
            var value = _fixture.Create<USINT>();
            
            FluentActions.Invoking(() => _converter.ConvertTo(value, typeof(SINT))!)
                .Should().Throw<NotSupportedException>();
        }

        [Test]
        public void ConvertTo_USint_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (USINT)_converter.ConvertTo(value, typeof(USINT));

            result.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_Int_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (INT)_converter.ConvertTo(value, typeof(INT));

            result?.Value.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_UInt_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (UINT)_converter.ConvertTo(value, typeof(UINT));

            result?.Value.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_Dint_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (DINT)_converter.ConvertTo(value, typeof(DINT));

            result?.Value.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_UDint_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (UDINT)_converter.ConvertTo(value, typeof(UDINT));

            result?.Value.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_Lint_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (LINT)_converter.ConvertTo(value, typeof(LINT));

            result?.Value.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_ULint_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (ULINT)_converter.ConvertTo(value, typeof(ULINT));

            result?.Value.Should().Be(value);
        }
        
        [Test]
        public void ConvertTo_Real_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (REAL)_converter.ConvertTo(value, typeof(REAL));

            result?.Value.Should().Be(value);
        }

        [Test]
        public void ConvertTo_string_ShouldBeExpected()
        {
            var value = _fixture.Create<USINT>();

            var result = (string)_converter.ConvertTo(value, typeof(string));

            result.Should().Be(value.Format());
        }
    }
}