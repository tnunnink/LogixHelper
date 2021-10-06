﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using L5Sharp.Abstractions;
using L5Sharp.Core;
using L5Sharp.Serialization;
using L5Sharp.Utilities;

namespace L5Sharp.Extensions
{
    public static class GenericL5XExtensions
    {
        private static readonly Dictionary<Type, IComponentSerializer> Serializers = new Dictionary<Type, IComponentSerializer>
        {
            { typeof(IDataType), new DataTypeSerializer() },
            { typeof(Member), new MemberSerializer() },
            { typeof(Tag), new TagSerializer() },
            { typeof(Program), new ProgramSerializer() },
            { typeof(Task), new TaskSerializer() }
        };
        
        private static readonly Dictionary<Type, string> Components = new Dictionary<Type, string>
        {
            { typeof(DataType), L5XNames.Components.DataType },
            { typeof(Module), L5XNames.Components.Module },
            { typeof(Instruction), L5XNames.Components.AddOnInstructionDefinition },
            { typeof(Tag), L5XNames.Components.Tag },
            { typeof(Program), L5XNames.Components.Program },
            { typeof(Task), L5XNames.Components.Task }
        };
        
        private static readonly Dictionary<Type, string> Containers = new Dictionary<Type, string>
        {
            { typeof(DataType), L5XNames.Containers.DataTypes },
            { typeof(Module), L5XNames.Containers.Modules },
            { typeof(Instruction), L5XNames.Containers.AddOnInstructions },
            { typeof(Tag), L5XNames.Containers.Tags },
            { typeof(Program), L5XNames.Containers.Programs },
            { typeof(Task), L5XNames.Containers.Tasks },
        };
        

        public static XElement Serialize<T>(this T component) where T : IComponent
        {
            var type = typeof(T);

            if (!Serializers.ContainsKey(type))
                throw new InvalidOperationException($"No serializer defined for type '{type}'");

            var serializer = (IComponentSerializer<T>)Serializers[type];

            return serializer.Serialize(component);
        }

        public static T Deserialize<T>(this XElement element) where T : IComponent
        {
            var type = typeof(T);

            if (!Serializers.ContainsKey(type))
                throw new InvalidOperationException($"Not serializer defined for type '{type}'");

            var serializer = (IComponentSerializer<T>)Serializers[type];

            return serializer.Deserialize(element);
        }
        
        public static bool Contains<T>(this XElement element, string name) where T : IComponent
        {
            var component = GetComponentName<T>();
            return element.Descendants(component).FirstOrDefault(x => x.GetName() == name) != null;
        }
        
        public static IEnumerable<XElement> GetAll<T>(this XElement element) where T : IComponent
        {
            var component = GetComponentName<T>();
            return element.Descendants(component);
        }

        public static XElement GetFirst<T>(this XElement element, string name) where T : IComponent
        {
            var component = GetComponentName<T>();
            return element.Descendants(component).FirstOrDefault(x => x.GetName() == name);
        }

        public static XElement GetSingle<T>(this XElement element, string name) where T : IComponent
        {
            var component = GetComponentName<T>();
            return element.Descendants(component).SingleOrDefault(x => x.GetName() == name);
        }
        
        public static XElement Container<T>(this XElement element) where T : IComponent
        {
            var container = GetContainerName<T>();
            return element.Descendants(container).FirstOrDefault();
        }
        
        private static string GetComponentName<T>() where T : IComponent
        {
            if (!Components.ContainsKey(typeof(T)))
                throw new InvalidOperationException($"No component name defined for type '{typeof(T)}'");

            return Components[typeof(T)];
        }
        
        private static string GetContainerName<T>() where T : IComponent
        {
            if (!Containers.ContainsKey(typeof(T)))
                throw new InvalidOperationException($"No container name defined for type '{typeof(T)}'");

            return Containers[typeof(T)];
        }
    }
}