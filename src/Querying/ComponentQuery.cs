﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using L5Sharp.Core;
using L5Sharp.Extensions;
using L5Sharp.Serialization;

namespace L5Sharp.Querying
{
    /// <inheritdoc cref="L5Sharp.Querying.IComponentQuery{TComponent}" />
    public class ComponentQuery<TComponent> : LogixQuery<TComponent>, IComponentQuery<TComponent>
        where TComponent : ILogixComponent
    {
        /// <summary>
        /// Creates a new <see cref="ComponentQuery{TComponent}"/> instance with the provided elements and serializer.
        /// </summary>
        /// <param name="elements">The source collection of elements for which to execute queries over.</param>
        /// <param name="serializer">The serializer instance that can deserialize elements to the specified result type.</param>
        protected ComponentQuery(IEnumerable<XElement> elements, IL5XSerializer<TComponent> serializer)
            : base(elements, serializer)
        {
        }

        /// <inheritdoc />
        public bool Any(ComponentName name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return Elements.Any(c => c.ComponentName() == name);
        }

        /// <inheritdoc />
        public TComponent? Named(ComponentName name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            var component = Elements.FirstOrDefault(e => e.ComponentName() == name);

            return component is not null ? Serializer.Deserialize(component) : default;
        }

        /// <inheritdoc />
        public IEnumerable<TComponent> Named(ICollection<ComponentName> names)
        {
            if (names is null)
                throw new ArgumentNullException(nameof(names));
            
            var components = Elements.Where(e => names.Contains(e.ComponentName()));
            
            return components.Select(e => Serializer.Deserialize(e));
        }

        /// <inheritdoc />
        public IEnumerable<ComponentName> Names() => Elements.Select(x => new ComponentName(x.ComponentName()));
    }
}