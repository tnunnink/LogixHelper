﻿using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace L5Sharp;

/// <summary>
/// A generic L5X markup exception to be thrown when a required attribute or element does not exist for the
/// provided <see cref="XElement"/> object.
/// </summary>
public class L5XException : XmlException
{
    /// <summary>
    /// Creates a new <see cref="L5XException"/> and generates the default message for the specified <see cref="XName"/> property.
    /// </summary>
    /// <param name="element">The element generating the exception.</param>
    /// <param name="name">The name of the property generating the exception.</param>
    public L5XException(string name, XElement element) : base(
        $"Invalid L5X element. The required property {name} does not exist for underlying element.")
    {
        Element = element;
    }

    /// <summary>
    /// Creates a new <see cref="L5XException"/> and generates the default message for the specified <see cref="XName"/> property.
    /// </summary>
    /// <param name="element">The element generating the exception.</param>
    /// <param name="name">The name of the property generating the exception.</param>
    public L5XException(XElement element, [CallerMemberName] string? name = null) : base(
        $"Invalid L5X element. The required property {name} does not exist for underlying element.")
    {
        Element = element;
    }

    /// <summary>
    /// The <see cref="XElement"/> that produced the exception.
    /// </summary>
    public XElement Element { get; }
}