﻿using AutoFixture.Kernel;
using L5Sharp.Types.Atomics;

namespace L5Sharp.Core.Tests.Specimens
{
    public class BoolGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is not Type type)
                return new NoSpecimen();

            if (type != typeof(BOOL))
                return new NoSpecimen();
            
            return new BOOL();
        }
    }
}