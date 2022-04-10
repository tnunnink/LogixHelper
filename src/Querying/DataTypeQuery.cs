﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using L5Sharp.Extensions;
using L5Sharp.L5X;
using L5Sharp.Serialization;

namespace L5Sharp.Querying
{
    internal class DataTypeQuery : ComponentQuery<IComplexType>, IDataTypeQuery
    {
        public DataTypeQuery(IEnumerable<XElement> elements, IL5XSerializer<IComplexType> serializer) 
            : base(elements, serializer)
        {
        }
        
        public IDataTypeQuery DependingOn(string typeName)
        {
            if (typeName is null)
                throw new ArgumentNullException(nameof(typeName));
            
            var results = Elements.Where(e =>
                e.Descendants(L5XElement.Member.ToString()).Any(x => x.DataTypeName() == typeName));

            return new DataTypeQuery(results, Serializer);
        }
    }
}