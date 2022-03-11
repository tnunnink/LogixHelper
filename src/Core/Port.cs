﻿using System;
using L5Sharp.Extensions;

namespace L5Sharp.Core
{
    /// <summary>
    /// A component of a <see cref="Module"/> that represents the means for connecting devices on a network or in a chassis.
    /// </summary>
    /// <remarks>
    /// A Port is a component that helps define the structure of the IO tree.
    /// The Port may be the slot on the chassis or backplane where the <see cref="Module"/> resides.
    /// This may also be the network address (IP) of the device. Each Port may also have a <see cref="Bus"/> that
    /// contains child Modules. Each Port is identifiable by the <see cref="Id"/> property. 
    /// </remarks>
    public sealed class Port
    {
        internal Port(Module module, PortDefinition definition)
        {
            Module = module ?? throw new ArgumentNullException(nameof(module));
            Id = definition.Id;
            Type = definition.Type ?? throw new ArgumentNullException(nameof(definition.Type));
            Upstream = definition.Upstream;
            Address = definition.Address;

            if (!Upstream && (Address.IsByte() || Address.IsIPv4()))
                Bus = new Bus(this, definition.BusSize);
        }

        /// <summary>
        /// Gets the <see cref="Module"/> instance that owns the current <see cref="Port"/>.
        /// </summary>
        public Module Module { get; }

        /// <summary>
        /// Gets the Id of the <see cref="Port"/>.
        /// </summary>
        /// <remarks>
        /// All Modules have at least one port. Each is identified by the Id property. Typically Modules will have one
        /// or two ports with Ids '1' and '2', respectively.
        /// </remarks>
        public int Id { get; }

        /// <summary>
        /// Gets the address of the <see cref="Port"/>.
        /// </summary>
        /// <remarks>
        /// The Address of a port represents the slot or IP address for the port. This value is used in the
        /// <see cref="Module"/> to determine the slot and IP properties. All ports must have an Address.
        /// </remarks>
        public string Address { get; }

        /// <summary>
        /// Gets the value that represents the <see cref="Port"/> type.
        /// </summary>
        /// <remarks>
        /// This value appears to be specific to the product. Ports with IP will have 'Network' for their type. This
        /// property is required for importing a <see cref="Module"/> correctly.
        /// </remarks>
        public string Type { get; }

        /// <summary>
        /// Gets the value indicating whether the there are devices upstream of the current <see cref="Port"/>.
        /// </summary>
        /// <remarks>
        /// From examining the L5X examples, the upstream flag seems to indicate that the port is one connected to a
        /// parent module living 'upstream' of the current device. Non-upstream ports seem to indicate they contain
        /// child modules, or connect to devices living 'downstream' of the current Module. This property must be set
        /// correctly to be able to import L5X.
        /// </remarks>
        public bool Upstream { get; }

        /// <summary>
        /// Gets the <see cref="Bus"/> for the <see cref="Port"/> object.
        /// </summary>
        /// <remarks>
        /// A port's bus represents the chassis or network on which Modules are accessible to the port.
        /// Only downstream modules will have a valid Bus. 
        /// </remarks>
        public Bus? Bus { get; }

        /// <summary>
        /// Generates a <see cref="PortDefinition"/> object containing the current port data.
        /// </summary>
        /// <returns>A new <see cref="PortDefinition"/> instance with the current port data.</returns>
        public PortDefinition ToDefinition() => new(Id, Type, Upstream, Address, Bus?.Size ?? default);
    }
}