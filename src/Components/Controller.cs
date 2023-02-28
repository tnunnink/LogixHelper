﻿using System;
using L5Sharp.Attributes;
using L5Sharp.Core;
using L5Sharp.Serialization;

namespace L5Sharp.Components
{
    /// <summary>
    /// A class representing the L5X controller component.
    /// </summary>
    [LogixSerializer(typeof(ControllerSerializer))]
    public class Controller : ILogixComponent
    {
        /// <inheritdoc />
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc />
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// The catalog number representing the processor of the controller component.
        /// </summary>
        /// <value>A <see cref="string"/> alpha numeric code.</value>
        public string ProcessorType { get; set; } = string.Empty;

        /// <summary>
        /// The revision or hardware version of the controller.
        /// </summary>
        /// <value>A <see cref="Core.Revision"/> value representing the major/minor version of the controller</value>
        public Revision? Revision { get; set; } = new();
        
        /// <summary>
        /// The date/time the current project was created. 
        /// </summary>
        /// <value>A <see cref="DateTime"/> representing the date and time of creation.</value>
        public DateTime ProjectCreationDate { get; set; }

        /// <summary>
        /// The date/time the current project was last modified. 
        /// </summary>
        /// <value>A <see cref="DateTime"/> representing the date and time of modification.</value>
        public DateTime LastModifiedDate { get; set; }
    }
}