﻿using FluentAssertions;
using L5Sharp.Types;
using NUnit.Framework;

namespace L5Sharp.Core.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void New_ValidName_ShouldNotBeNull()
        {
            var program = new Program("Test");

            program.Should().NotBeNull();
        }
        
        [Test]
        public void AddTag_ValidName_ShouldNotBeNull()
        {
            var program = new Program("Test");

            program.AddTag<Bool>("Test_Bool");

            program.Tags.Should().Contain(t => t.Name == "Test_Bool");
        }

        [Test]
        public void GetTagTyped_AsCorrectType_ShouldNotBeNull()
        {
            var program = new Program("Test");
            program.AddTag<Bool>("Test");

            var tag = program.GetTag<Bool>("Test");

            tag.Should().NotBeNull();
            tag.Name.Should().Be("Test");
        }
        
        [Test]
        public void GetTagTyped_AsDifferentType_ShouldBeNull()
        {
            var program = new Program("Test");
            program.AddTag<Bool>("Test");

            var tag = program.GetTag<Int>("Test");

            tag.Should().BeNull();
        }
        
        [Test]
        public void GetTagTyped_FromNonGenericTag_ShouldNotBeNull()
        {
            var program = new Program("Test");
            program.AddTag("Test", Predefined.Dint);

            var tag = program.GetTag<Dint>("Test");

            tag.Should().NotBeNull();
            tag.Name.Should().Be("Test");
        }
    }
}