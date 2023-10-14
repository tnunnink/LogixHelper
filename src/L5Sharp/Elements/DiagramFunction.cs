﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using L5Sharp.Utilities;

namespace L5Sharp.Elements;

/// <summary>
/// A <c>DiagramElement</c> type that defines the properties for built in logix function within a
/// Function Block Diagram (FBD).
/// </summary>
/// <footer>
/// See <a href="https://literature.rockwellautomation.com/idc/groups/literature/documents/rm/1756-rm084_-en-p.pdf">
/// `Logix 5000 Controllers Import/Export`</a> for more information.
/// </footer>
[L5XType(L5XName.Function, L5XName.Sheet)]
public class DiagramFunction : DiagramElement
{
    /// <summary>
    /// Creates a new <see cref="DiagramFunction"/> with default values.
    /// </summary>
    public DiagramFunction()
    {
    }

    /// <summary>
    /// Creates a new <see cref="DiagramFunction"/> initialized with the provided <see cref="XElement"/>.
    /// </summary>
    /// <param name="element">The <see cref="XElement"/> to initialize the type with.</param>
    /// <exception cref="ArgumentNullException"><c>element</c> is null.</exception>
    public DiagramFunction(XElement element) : base(element)
    {
    }

    /// <inheritdoc />
    public override string Location => Sheet is not null ? $"Sheet {Sheet.Number} {Cell}" : $"{Cell}";

    /// <summary>
    /// The mnemonic name specifying the type of <c>DiagramFunction</c>.
    /// </summary>
    /// <value>A <see cref="string"/> containing the type of the function if it exists; Otherwise, <c>null</c>.</value>
    public string? Type
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    /// <summary>
    /// The <see cref="Sheet"/> this <c>DiagramFunction</c> belongs to.
    /// </summary>
    /// <value>A <see cref="Sheet"/> representing the containing code FBD sheet.</value>
    public Sheet? Sheet => Element.Parent is not null ? new Sheet(Element.Parent) : default;
    
    /// <inheritdoc />
    public override IEnumerable<LogixReference> References()
    {
        if (Type is not null)
            yield return new LogixReference(Element, Type, L5XName.AddOnInstructionDefinition);
    }
}