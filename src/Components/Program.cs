﻿using System.Collections.Generic;
using System.Xml.Linq;
using L5Sharp.Enums;

namespace L5Sharp.Components
{
    /// <summary>
    /// 
    /// </summary>
    /// <footer>
    /// See <a href="https://literature.rockwellautomation.com/idc/groups/literature/documents/rm/1756-rm084_-en-p.pdf">
    /// `Logix 5000 Controllers Import/Export`</a> for more information.
    /// </footer>
    public class Program : ILogixComponent
    {
        /// <inheritdoc />
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc />
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets the type of the program (Normal, Equipment Phase).
        /// </summary>
        /// <value>A <see cref="Enums.ProgramType"/> enum representing the type of the program.</value>
        public ProgramType Type { get; set; } = ProgramType.Normal;
        
        /// <summary>
        /// The value indicating whether the program has current test edits pending.
        /// </summary>
        /// <value>>A <see cref="bool"/>; <c>true</c>if the program has test edits; otherwise <c>false</c>.</value>
        public bool TestEdits { get; set; } = false;

        /// <summary>
        /// The value indicating whether the program is disabled (or inhibited).
        /// </summary>
        /// <value>A <see cref="bool"/>; <c>true</c> if the program is disabled; otherwise <c>false</c>.</value>
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// The name of the routine that serves as the entry point for the program (i.e. main routine).
        /// </summary>
        /// <value>A <see cref="string"/> representing the name of the main routine for the program.</value>
        public string MainRoutineName { get; set; } = string.Empty;

        /// <summary>
        /// The name of the routine that serves as the fault routine for the program.
        /// </summary>
        /// <value>A <see cref="string"/> representing the name of the fault routine for the program.</value>
        public string FaultRoutineName { get; set; } = string.Empty;

        /// <summary>
        /// A flag indicating whether the program is used as a folder or container for other programs,
        /// as opposed to a container of tags and logix.
        /// </summary>
        /// <value>A <see cref="bool"/>; <c>true</c> if the program is a folder; otherwise, <c>false</c>.</value>
        public bool UseAsFolder { get; set; } = false;

        /// <summary>
        /// A collection of locally scoped <see cref="Tag"/> components contained by the program. 
        /// </summary>
        /// <value>A <see cref="Dictionary{TKey,TValue}"/> containing the collection of <see cref="Tag"/> objects.</value>
        public Dictionary<string, Tag> Tags { get; set; } = new();

        /// <summary>
        /// A collection of locally scoped <see cref="Routine"/> components contained by the program. 
        /// </summary>
        /// <value>A <see cref="Dictionary{TKey,TValue}"/> containing the collection of <see cref="Routine"/> objects.</value>
        public Dictionary<string, Routine> Routines { get; set; } = new();

        /// <inheritdoc />
        public XElement Serialize()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void Deserialize(XElement element)
        {
            throw new System.NotImplementedException();
        }
    }
}