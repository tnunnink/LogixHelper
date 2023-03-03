﻿using System;
using FluentAssertions;
using L5Sharp.Extensions;
using L5Sharp.Types;
using L5Sharp.Types.Atomics;
using L5Sharp.Types.Predefined;
using NUnit.Framework;

namespace L5Sharp.Tests.Extensions
{
    [TestFixture]
    public class LogixTypeExtensionTests
    {
        [Test]
        public void ToType_ValidTypes_ShouldBeOfExpectedType()
        {
            var type = (ILogixType)new BOOL();

            var result = type.ToType<BOOL>();

            result.Should().BeOfType<BOOL>();
        }
        
        [Test]
        public void ToType_InvalidTypes_ShouldThrowException()
        {
            var type = (ILogixType)new BOOL();

            FluentActions.Invoking(() => type.ToType<DINT>()).Should().Throw<InvalidCastException>();
        }
        
        [Test]
        public void AsType_InvalidTypes_ShouldBeNull()
        {
            var type = (ILogixType)new BOOL();

            var result = type.AsType<DINT>();

            result.Should().BeNull();
        }

        [Test]
        public void ToArrayType_WhenCalled_ShouldBeOfType()
        {
            var array = new ArrayType<BOOL>(new[] { new BOOL(), new BOOL(), new BOOL() });

            var generic = array.ToArrayType();

            generic.Should().NotBeNull();
            generic.Should().BeOfType<ArrayType<ILogixType>>();
        }

        [Test]
        public void ToArrayType_OfSpecificType_ShouldBeOfExpectedType()
        {
            var array = new ArrayType<TIMER>(new[] { new TIMER(), new TIMER(), new TIMER() });

            var generic = array.ToArrayType<TIMER, StructureType>();

            generic.Should().NotBeNull();
            generic.Should().BeOfType<ArrayType<StructureType>>();
        }
    }
}