﻿using System;
using System.Linq;
using System.Xml.Linq;
using L5Sharp.Core;
using L5Sharp.Enums;
using L5Sharp.Extensions;
using L5Sharp.L5X;

namespace L5Sharp.Serialization.Data
{
    internal class StructureSerializer : IL5XSerializer<IComplexType>
    {
        private static readonly XName ElementName = L5XElement.Structure.ToString();
        private readonly IL5XSerializer<IMember<IDataType>> _dataValueMemberSerializer;
        private readonly IL5XSerializer<IMember<IDataType>> _arrayMemberSerializer;
        private readonly IL5XSerializer<IMember<IDataType>> _structureMemberSerializer;

        public StructureSerializer()
        {
            _dataValueMemberSerializer = new DataValueMemberSerializer();
            _arrayMemberSerializer = new ArrayMemberSerializer(this);
            _structureMemberSerializer = new StructureMemberSerializer(this);
        }

        public XElement Serialize(IComplexType component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            var element = new XElement(ElementName);

            element.Add(new XAttribute(L5XElement.DataType.ToString(), component.Name));

            var members = component.Members.Select(m => m.IsValueMember ? _dataValueMemberSerializer.Serialize(m) 
                : m.IsArrayMember ? _arrayMemberSerializer.Serialize(m) 
                : m.IsStructureMember ? _structureMemberSerializer.Serialize(m)
                : throw new InvalidOperationException("Could not determine member type."));

            element.Add(members);

            return element;
        }

        public IComplexType Deserialize(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (element.Name != ElementName)
                throw new ArgumentException($"Element '{element.Name}' not valid for the serializer {GetType()}.");

            var name = element.DataTypeName();
            var members = element.Elements().Select(e => GetSerializer(e).Deserialize(e)).ToList();

            return new StructureType(name, DataTypeClass.Unknown, members);
        }
        
        private IL5XSerializer<IMember<IDataType>> GetSerializer(XElement element)
        {
            return element.Name == L5XElement.DataValueMember.ToString() ? _dataValueMemberSerializer
                : element.Name == L5XElement.ArrayMember.ToString() ? _dataValueMemberSerializer
                : element.Name == L5XElement.StructureMember.ToString() ? _structureMemberSerializer
                : throw new ArgumentException();
        }
    }
}