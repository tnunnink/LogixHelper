﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using L5Sharp.Serialization;
using L5Sharp.Utilities;

namespace L5Sharp.Extensions
{
    internal static class SerializerExtensions
    {
        private static readonly Dictionary<string, IXSerializer> Serializers = new Dictionary<string, IXSerializer>
        {
            { LogixNames.Controller, new ControllerSerializer() },
            { LogixNames.DataType, new UserDefinedSerializer() },
            { LogixNames.Member, new UserDefinedMemberSerializer() },
            { LogixNames.Tag, new TagSerializer() },
            { LogixNames.Task, new TaskSerializer() },
            { LogixNames.DataValue, new DataValueSerializer() },
            { LogixNames.Array, new ArraySerializer() },
            { LogixNames.Structure, new StructureSerializer() },
            { LogixNames.DataValueMember, new DataValueMemberSerializer() },
            { LogixNames.ArrayMember, new ArrayMemberSerializer() },
            { LogixNames.StructureMember, new StructureMemberSerializer() }
        };
        
        public static XElement Serialize<T>(this T component, string serializerName = null)
        {
            var name = serializerName ?? LogixNames.GetComponentName<T>();
            var serializer = GetSerializer<T>(name);
            return serializer.Serialize(component);
        }

        public static T Deserialize<T>(this XElement element)
        {
            var serializer = GetSerializer<T>(element.Name.ToString());
            return serializer.Deserialize(element);
        }

        private static IXSerializer<T> GetSerializer<T>(string name)
        {
            if (!Serializers.TryGetValue(name, out var serializer))
                throw new InvalidOperationException($"Serializer not defined for component '{name}'");

            return (IXSerializer<T>)serializer;
        }
    }
}