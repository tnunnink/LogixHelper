﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using L5Sharp.Enumerations;
using L5Sharp.Exceptions;
using NUnit.Framework;

namespace L5Sharp.Core.Tests
{
    [TestFixture]
    public class DataTypeTests
    {
        [Test]
        public void New_NullName_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => new DataType(string.Empty)).Should()
                .Throw<ArgumentException>();
        }
        
        [Test]
        public void New_EmptyName_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => new DataType(string.Empty)).Should()
                .Throw<ArgumentException>();
        }

        [Test]
        public void New_InvalidName_ShouldThrowInvalidTagNameException()
        {
            var fixture = new Fixture();
            FluentActions.Invoking(() => new DataType(fixture.Create<string>())).Should()
                .Throw<InvalidNameException>();
        }

        [Test]
        public void New_ValidNameAndDescription_ShouldBeExpected()
        {
            var fixture = new Fixture();
            var description = fixture.Create<string>();

            var type = new DataType("Test", description);

            type.Should().NotBeNull();
            type.Name.Should().Be("Test");
            type.Class.Should().Be(DataTypeClass.User);
            type.Family.Should().Be(DataTypeFamily.None);
            type.Description.Should().Be(description);
            type.IsAtomic.Should().BeFalse();
            type.Members.Should().BeEmpty();
        }
        
        [Test]
        public void New_MemberOverload_ShouldBeExpected()
        {
            var fixture = new Fixture();
            var description = fixture.Create<string>();
            var member = new Member("Member", Predefined.Bool);

            var type = new DataType("Test", member, description);

            type.Should().NotBeNull();
            type.Name.Should().Be("Test");
            type.Class.Should().Be(DataTypeClass.User);
            type.Family.Should().Be(DataTypeFamily.None);
            type.Description.Should().Be(description);
            type.IsAtomic.Should().BeFalse();
            type.Members.Should().HaveCount(1);
            type.Members.Should().Contain(member);
        }
        
        [Test]
        public void New_MembersOverload_ShouldBeExpected()
        {
            var fixture = new Fixture();
            var description = fixture.Create<string>();
            var members = new List<Member>
            {
                new("Member01", Predefined.Bool),
                new("Member02", Predefined.Bool)
            };

            var type = new DataType("Test", members, description);

            type.Should().NotBeNull();
            type.Name.Should().Be("Test");
            type.Class.Should().Be(DataTypeClass.User);
            type.Family.Should().Be(DataTypeFamily.None);
            type.Description.Should().Be(description);
            type.IsAtomic.Should().BeFalse();
            type.Members.Should().HaveCount(2);
            type.Members.Should().BeEquivalentTo(members);
        }
        
        [Test]
        public void New_MembersOverloadBools_ShouldBeExpected()
        {
            var fixture = new Fixture();
            var description = fixture.Create<string>();
            var members = new List<Member>
            {
                new("Member01", Predefined.Bool),
                new("Member02", Predefined.Bool),
                new("Member03", Predefined.Bool),
                new("Member04", Predefined.Bool),
                new("Member05", Predefined.Int),
                new("Member06", Predefined.Bool),
                new("Member07", Predefined.Bool),
                new("Member08", Predefined.Real),
                new("Member09", Predefined.Bool),
                new("Member10", Predefined.Bool)
            };

            var type = new DataType("Test", members, description);

            type.Should().NotBeNull();
            type.Members.Should().HaveCount(10);
            type.Members.Should().BeEquivalentTo(members);
        }

        [Test]
        public void Name_SetValueValidName_ShouldBeExpectedValue()
        {
            var fixture = new Fixture();
            var description = fixture.Create<string>();
            var type = new DataType("Test", description);

            type.Name = "NewName";

            type.Name.Should().Be("NewName");
        }

        [Test]
        public void Name_SetValueInvalidName_ShouldThrowInvalidTagNameException()
        {
            var fixture = new Fixture();
            var description = fixture.Create<string>();
            var type = new DataType("Test", description);

            FluentActions.Invoking(() => type.Name = "Not.Valid%01").Should().Throw<InvalidNameException>();
        }

        [Test]
        public void Description_SetValueValidValue_ShouldBeExpectedValue()
        {
            var fixture = new Fixture();
            var description = fixture.Create<string>();
            var newDescription = fixture.Create<string>();
            var type = new DataType("Test", description);

            type.Description = newDescription;

            type.Description.Should().Be(newDescription);
        }

        [Test]
        public void GetMember_ValidName_ShouldNotBeNull()
        {
            var type = new DataType("Test");
            
            
        }
        
        [Test]
        public void Description_SetValueNull_ShouldThrowArgumentNullException()
        {
            var type = new DataType("Test");

            FluentActions.Invoking(() => type.Description = null).Should().Throw<ArgumentNullException>();
        }
        
        

        [Test]
        public void AddMember_ExistingMember_ShouldThrowMemberNameCollisionException()
        {
            var member = new Member("Test", Predefined.Dint);
            var type = new DataType("Test");

            FluentActions.Invoking(() => type.AddMember("Member", DataType.Int)).Should()
                .Throw<ComponentNameCollisionException>();
        }

        [Test]
        public void AddMember_InvalidName_ShouldThrowInvalidNameException()
        {
            var fixture = new Fixture();
            var type = new DataType("Test");

            FluentActions.Invoking(() => type.AddMember(fixture.Create<string>(), DataType.Dint)).Should()
                .Throw<InvalidNameException>();
        }
        
        [Test]
        public void AddMember_NullName_ShouldThrowArgumentNullException()
        {
            var type = new DataType("Test");

            FluentActions.Invoking(() => type.AddMember(null, DataType.Dint)).Should()
                .Throw<ArgumentNullException>();
        }

        [Test]
        public void AddMember_NullDataType_ShouldHaveMemberWithNullType()
        {
            var type = new DataType("Test");

            type.AddMember("Member", null);

            var member = type.GetMember("Member");
            member.DataType.Should().Be(DataType.Null);
        }

        [Test]
        public void AddMember_InvalidRadixForType_ShouldThrowRadixNotSupportedException()
        {
            var type = new DataType("Test");

            FluentActions.Invoking(() => type.AddMember("Member", DataType.Int, radix: Radix.Float)).Should()
                .Throw<RadixNotSupportedException>();
        }

        [Test]
        public void AddMember_ValidArguments_ShouldHaveExpectedMember()
        {
            var type = new DataType("Test");

            type.AddMember("Member", DataType.Dint);

            type.Members.Should().HaveCount(1);
            var member = type.GetMember("Member");
            member.Should().NotBeNull();
            member.Name.Should().Be("Member");
            member.DataType.Should().Be(DataType.Dint);
            member.Dimension.Should().Be(0);
            member.Description.Should().Be(string.Empty);
            member.Radix.Should().Be(Radix.Decimal);
            member.ExternalAccess.Should().Be(ExternalAccess.ReadWrite);
        }

        [Test]
        public void AddMember_BooleanMember_ShouldHaveBackingMember()
        {
            var type = new DataType("Test");
            
            type.AddMember("Member", DataType.Bool);

            type.Members.Should().HaveCount(1);
        }

        [Test]
        public void RemoveMember_ExistingMember_MembersShouldEmpty()
        {
            var type = new DataType("Test_Type_001");
            type.AddMember("Member", DataType.Dint);

            type.RemoveMember("Member");

            type.Members.Should().BeEmpty();
        }

        [Test]
        public void RemoveMember_NonExistingMember_MembersShouldSame()
        {
            var type = new DataType("Test_Type_001");
            type.AddMember("Member", DataType.Dint);

            type.RemoveMember("Test");

            type.Members.Should().HaveCount(1);
        }

        [Test]
        public void GetMember_ExistingName_ShouldNotBeNullAndExpected()
        {
            var fixture = new Fixture();
            var description = fixture.Create<string>();
            var type = new DataType("Test", description);
            type.AddMember("Member", DataType.Dint);

            var member = type.GetMember("Member");

            member.Should().NotBeNull();
            member.Name.Should().Be("Member");
            member.DataType.Should().Be(DataType.Dint);
        }

        [Test]
        public void GetDependentTypes_ComplexType_ShouldContainExpectedTypes()
        {
            var type = new DataType("Type01");
            type.AddMember("Member01", DataType.Bool);
            type.AddMember("Member02", DataType.Counter);
            type.AddMember("Member03", DataType.Dint);
            type.AddMember("Member04", DataType.String);
            var dependentUserType = new DataType("Type02");
            dependentUserType.AddMember("Member01", DataType.Bool);
            dependentUserType.AddMember("Member02", DataType.Dint);
            dependentUserType.AddMember("Member03", DataType.String);
            dependentUserType.AddMember("Member04", DataType.Timer);
            type.AddMember("Member05", dependentUserType);

            var dependents = type.GetDependentTypes().ToList();

            dependents.Should().NotBeEmpty();
            dependents.Should().Contain(DataType.Bool);
            dependents.Should().Contain(DataType.Counter);
            dependents.Should().Contain(DataType.Dint);
            dependents.Should().Contain(DataType.String);
            dependents.Should().Contain(DataType.Timer);
            dependents.Should().Contain(dependentUserType);
        }
        
        [Test]
        public void GetDependentUserTypes_ComplexType_ShouldContainExpectedTypes()
        {
            var type = new DataType("Type01");
            type.AddMember("Member01", DataType.Bool);
            type.AddMember("Member02", DataType.Counter);
            type.AddMember("Member03", DataType.Dint);
            type.AddMember("Member04", DataType.String);
            var dependentUserType = new DataType("Type02");
            dependentUserType.AddMember("Member01", DataType.Bool);
            dependentUserType.AddMember("Member02", DataType.Dint);
            dependentUserType.AddMember("Member03", DataType.String);
            dependentUserType.AddMember("Member04", DataType.Timer);
            type.AddMember("Member05", dependentUserType);

            var dependents = type.GetDependentUserTypes().ToList();

            dependents.Should().NotBeEmpty();
            dependents.Should().HaveCount(1);
            dependents.Should().Contain(dependentUserType);
        }
    }
}