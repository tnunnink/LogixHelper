﻿using FluentAssertions;
using L5Sharp.Comparers;
using L5Sharp.Components;
using L5Sharp.Types;
using NUnit.Framework;

namespace L5Sharp.Common.Tests
{
    [TestFixture]
    public class ComponentNameComparerTests
    {
        [Test]
        public void Equals_EqualNameDifferentTypes()
        {
            var t1 = new Bool();
            var t2 = Member.Create<Bool>("BOOL");

            var comparer = ComponentNameComparer.Instance;

            comparer.Equals(t1, t2).Should().BeTrue();
        }
    }
}