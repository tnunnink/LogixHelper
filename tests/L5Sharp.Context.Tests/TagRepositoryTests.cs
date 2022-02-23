﻿using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace L5Sharp.Context.Tests
{
    [TestFixture]
    public class TagRepositoryTests
    {
        [Test]
        public void Contains_ValidComponent_ShouldBeTrue()
        {
            var context = new L5XContext(Known.L5X);

            var result = context.Tags.Contains("SimpleDint");

            result.Should().BeTrue();
        }

        [Test]
        public void Contains_InvalidComponent_ShouldBeFalse()
        {
            var context = new L5XContext(Known.L5X);

            var result = context.Tags.Contains("FakeTag");

            result.Should().BeFalse();
        }

        [Test]
        public void Contains_Null_ShouldBeFalse()
        {
            var context = new L5XContext(Known.L5X);

            var result = context.Tags.Contains(null!);

            result.Should().BeFalse();
        }
        
        [Test]
        public void Find_ComponentName_ExistingName_ShouldNotBeNull()
        {
            var context = new L5XContext(Known.L5X);

            var component = context.Tags.Find("SimpleDint");

            component.Should().NotBeNull();
        }

        [Test]
        public void Find_ComponentName_NonExistingName_ShouldBeNull()
        {
            var context = new L5XContext(Known.L5X);

            var component = context.Tags.Find("FakeType");

            component.Should().BeNull();
        }
        
        [Test]
        public void Find_Predicate_HasComponents_ShouldNotBeNull()
        {
            var context = new L5XContext(Known.L5X);

            var component = context.Tags.GetAll().Where(t => t.Dimensions.AreEmpty);

            component.Should().NotBeEmpty();
        }

        [Test]
        public void FindAll_Predicate_HasComponents_ShouldNotBeEmpty()
        {
            var context = new L5XContext(Known.L5X);

            var components = context.DataTypes.FindAll(t => t.Name.Contains("Simple")).ToList();

            components.Should().NotBeEmpty();
            components.All(c => c.Name.Contains("Simple")).Should().BeTrue();
        }

        [Test]
        public void GetAll_WhenCalled_shouldNotBeEmpty()
        {
            var context = new L5XContext(Known.L5X);

            var tags = context.Tags.GetAll();

            tags.Should().NotBeEmpty();
        }
    }
}