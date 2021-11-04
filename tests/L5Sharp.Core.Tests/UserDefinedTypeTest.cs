﻿using System.Linq;
using FluentAssertions;
using L5Sharp.Abstractions;
using L5Sharp.Enums;
using NUnit.Framework;

namespace L5Sharp.Core.Tests
{
    [TestFixture]
    public class UserDefinedTypeTest : DataType
    {
        public UserDefinedTypeTest() : base(nameof(UserDefinedTypeTest), "My Type description")
        {
            Members.Add(new DataTypeMember(nameof(MyMember01), Logix.DataType.Bool, description: "This is a test member"));
            Members.Add(new DataTypeMember(nameof(MyMember02), Logix.DataType.Dint, new Dimensions(5), Radix.Ascii,
                description: "This is a test member array"));
        }

        public IMember MyMember01 => Members.SingleOrDefault(m => m.Name == nameof(MyMember01));
        public IMember MyMember02 => Members.SingleOrDefault(m => m.Name == nameof(MyMember02));


        [Test]
        public void ShouldBeInitializedAsExpected()
        {
            Name.Should().Be("UserDefinedTypeTest");
            Description.Should().Be("My Type description");
            Family.Should().Be(DataTypeFamily.None);
            Class.Should().Be(DataTypeClass.User);

            MyMember01.Should().NotBeNull();
            MyMember01.DataType.Should().Be(Logix.DataType.Bool);
            MyMember01.Description.Should().Be("This is a test member");
            MyMember01.Dimensions.Length.Should().Be(0);
            MyMember01.Radix.Should().Be(Radix.Decimal);
            MyMember01.ExternalAccess.Should().Be(ExternalAccess.ReadWrite);

            MyMember02.Should().NotBeNull();
            MyMember02.DataType.Should().Be(Logix.DataType.Dint);
            MyMember02.Description.Should().Be("This is a test member array");
            MyMember02.Dimensions.Length.Should().Be(5);
            MyMember02.Radix.Should().Be(Radix.Ascii);
            MyMember02.ExternalAccess.Should().Be(ExternalAccess.ReadWrite);
        }
    }
}