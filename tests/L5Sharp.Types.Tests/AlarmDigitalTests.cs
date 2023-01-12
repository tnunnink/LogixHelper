﻿using FluentAssertions;
using L5Sharp.Enums;
using L5Sharp.Types.Predefined;
using NUnit.Framework;

namespace L5Sharp.Types.Tests
{
    [TestFixture]
    public class AlarmDigitalTests
    {
        [Test]
        public void Constructor_WhenCalled_ShouldNotBeNull()
        {
            var type = new ALARM_DIGITAL();

            type.Should().NotBeNull();
        }
        
        [Test]
        public void Class_GetValue_ShouldBeExpected()
        {
            var type = new ALARM_DIGITAL();
            
            type.Class.Should().Be(DataTypeClass.Predefined);
        }
    }
}