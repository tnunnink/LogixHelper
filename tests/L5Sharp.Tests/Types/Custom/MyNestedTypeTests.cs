﻿using L5Sharp.Types.Atomics;
using NUnit.Framework;

namespace L5Sharp.Tests.Types.Custom
{
    [TestFixture]
    public class MyNestedTypeTests
    {
        [Test]
        public void Testing()
        {
            var nested = new MyNestedType();
            
            nested.Simple.M1= true;
            ;
            nested.Tmr.PRE = new DINT(5000);
            
            var flag = nested.Flags[2];

            var message = nested.Messages[4];
        }
    }
}