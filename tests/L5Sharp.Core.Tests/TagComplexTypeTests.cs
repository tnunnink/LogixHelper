﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using L5Sharp.Components;
using L5Sharp.Enums;
using L5Sharp.Types;
using NUnit.Framework;

namespace L5Sharp.Core.Tests
{
    [TestFixture]
    public class TagComplexTypeTests
    {
        [Test]
        public void Create_Timer_ShouldHaveExpectedValues()
        {
            var tag = Tag.Create<Timer>("Test");

            tag.Name.Should().Be("Test");
            tag.Description.Should().BeEmpty();
            tag.TagName.Should().Be("Test");
            tag.DataType.Should().BeOfType<Timer>();
            tag.Dimensions.Should().Be(Dimensions.Empty);
            tag.Radix.Should().Be(Radix.Null);
            tag.ExternalAccess.Should().Be(ExternalAccess.None);
            tag.TagType.Should().Be(TagType.Base);
            tag.Usage.Should().Be(TagUsage.Null);
            tag.Constant.Should().BeFalse();
            tag.IsValueMember.Should().BeFalse();
            tag.IsStructureMember.Should().BeTrue();
            tag.IsArrayMember.Should().BeFalse();
        }
        
        [Test]
        public void GetTagNames_Timer_ShouldHaveExpectedCount()
        {
            var tag = Tag.Create("Test", new Timer());

            var tagNames = tag.GetTagNames();

            tagNames.Should().HaveCount(5);
        }

        [Test]
        public void GetTagNames_Timer_ShouldHaveExpectedNames()
        {
            var tag = Tag.Create("Test", new Timer());

            var tagNames = tag.GetTagNames().ToList();

            tagNames.Should().Contain("Test.PRE");
            tagNames.Should().Contain("Test.ACC");
            tagNames.Should().Contain("Test.EN");
            tagNames.Should().Contain("Test.TT");
            tagNames.Should().Contain("Test.DN");
        }

        [Test]
        public void Contains_Null_ShouldBeFalse()
        {
            var tag = Tag.Create<Timer>("Test");

            var result = tag.Contains(null!);

            result.Should().BeFalse();
        }

        [Test]
        public void Contains_ValidExistingTagName_ShouldBeTrue()
        {
            var tag = Tag.Create<Timer>("Test");

            var result = tag.Contains("Test.PRE");

            result.Should().BeTrue();
        }

        [Test]
        public void NameIndex_Null_ShouldThrowArgumentNullException()
        {
            var tag = Tag.Create<Timer>("Test");

            FluentActions.Invoking(() => tag[null!]).Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void NameIndex_EmptyString_ShouldThrowArgumentException()
        {
            var tag = Tag.Create<Timer>("Test");

            FluentActions.Invoking(() => tag[string.Empty]).Should().Throw<ArgumentException>()
                .WithMessage("TagName can not be null or empty.");
        }

        [Test]
        public void NameIndex_NonExistingMember_ShouldThrowArgumentException()
        {
            var tag = Tag.Create<Timer>("Test");

            FluentActions.Invoking(() => tag["Invalid"]).Should().Throw<KeyNotFoundException>()
                .WithMessage($"The member 'Invalid' does not exist as a valid descendent of '{typeof(Timer)}'");
        }

        [Test]
        public void NameIndex_ValidRelativeHasMember_ShouldNotBeNull()
        {
            var tag = Tag.Create<Timer>("Test");

            var member = tag["PRE"];

            member.Should().NotBeNull();
        }
        
        [Test]
        public void NameIndex_ValidAbsoluteHasMember_ShouldNotBeNull()
        {
            var tag = Tag.Create<Timer>("Test");

            var member = tag["Test.PRE"];

            member.Should().NotBeNull();
        }

        [Test]
        public void NameIndex_ValidNested_ShouldBeExpected()
        {
            var tag = Tag.Create<MyNestedType>("Test");

            var member = tag["Tmr.PRE"];

            member.Should().NotBeNull();
            member.Name.Should().Be("PRE");
            member.DataType.Should().BeOfType<Dint>();
        }

        [Test]
        public void NameIndex_ChainedCalls_ShouldBeExpected()
        {
            var tag = Tag.Create<MyNestedType>("Test");

            var member = tag["Tmr"]["PRE"];

            member.Should().NotBeNull();
            member.Name.Should().Be("PRE");
            member.DataType.Should().BeOfType<Dint>();
        }

        [Test]
        public void GetMember_ValidMember_ShouldNotBeNull()
        {
            var tag = Tag.Create<Timer>("Test");

            var member = tag.GetMember(t => t.PRE);

            member.Should().NotBeNull();
        }

        [Test]
        public void GetMember_ChainedCalls_ShouldNotBeNull()
        {
            var tag = Tag.Create<MyNestedType>("Test");

            var member = tag
                .GetMember(t => t.Str)
                .GetMember(t => t.DATA)
                .GetMember(t => t[0]);

            member.Should().NotBeNull();
            member.Name.Should().Be("[0]");
            member.DataType.Should().BeOfType<Sint>();
        }

        [Test]
        public void GetMembers_WhenCalled_ShouldNotBeEmpty()
        {
            var tag = Tag.Create<Timer>("Test");

            var members = tag.GetMembers();

            members.Should().HaveCount(5);
        }
    }
}