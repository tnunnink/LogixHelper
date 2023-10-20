﻿using System;
using System.Collections.Generic;
using L5Sharp.Enums;
using L5Sharp.Types.Atomics;
using L5Sharp.Utilities;

namespace L5Sharp.Types;

/// <summary>
/// A static factory for <see cref="AtomicType"/> objects.
/// </summary>
public static class Atomic
{
    private static readonly Dictionary<string, Func<string, AtomicType>> Atomics = new()
    {
        { nameof(BOOL), BOOL.Parse },
        { "BIT", BOOL.Parse },
        { nameof(SINT), SINT.Parse },
        { nameof(INT), INT.Parse },
        { nameof(DINT), DINT.Parse },
        { nameof(LINT), LINT.Parse },
        { nameof(REAL), REAL.Parse },
        { nameof(LREAL), LREAL.Parse },
        { nameof(USINT), USINT.Parse },
        { nameof(UINT), UINT.Parse },
        { nameof(UDINT), UDINT.Parse },
        { nameof(ULINT), ULINT.Parse }
    };

    /// <summary>
    /// Parses the provided string value into an atomic type value.
    /// </summary>
    /// <param name="value">The value of the atomic type.</param>
    /// <returns>A <see cref="AtomicType"/> representing the value and format of the provided value.</returns>
    /// <exception cref="FormatException"><c>value</c> does not have a valid format to be parsed as an atomic type.</exception>
    public static AtomicType Parse(string value)
    {
        return value.IsEquivalent("true") ? new BOOL(true)
            : value.IsEquivalent("false") ? new BOOL()
            : Radix.Infer(value).Parse(value);
    }

    /// <summary>
    /// Parses the provided string value into an atomic type value.
    /// </summary>
    /// <param name="value">The value of the atomic type.</param>
    /// <returns>A <see cref="AtomicType"/> representing the value and format of the provided value.</returns>
    /// <exception cref="FormatException"><c>value</c> does not have a valid format to be parsed as an atomic type.</exception>
    public static TAtomic Parse<TAtomic>(string value) where TAtomic : AtomicType
    {
        var atomic = value.IsEquivalent("true") ? new BOOL(true)
            : value.IsEquivalent("false") ? new BOOL()
            : Radix.Infer(value).Parse(value);
        return (TAtomic)Convert.ChangeType(atomic, typeof(TAtomic));
    }

    /// <summary>
    /// Parses the provided string value into the atomic type value specified by name.
    /// </summary>
    /// <param name="name">The name of the atomic type.</param>
    /// <param name="value">The value of the atomic type.</param>
    /// <returns>A <see cref="AtomicType"/> representing the value and format of the provided value.</returns>
    /// <exception cref="InvalidOperationException"><c>name</c> does not represent a valid atomic type.</exception>
    /// <exception cref="FormatException"><c>value</c> does not have a valid format to be parsed as the specified atomic type.</exception>
    public static AtomicType Parse(string name, string value)
    {
        if (!Atomics.ContainsKey(name))
            throw new ArgumentException($"The type name '{name}' is not a valid {typeof(AtomicType)}");

        return Atomics[name].Invoke(value);
    }

    /// <summary>
    /// Attempts to parse the provided string input as an atomic type value with the inferred radix format.
    /// </summary>
    /// <param name="value">A string representing an atomic value to parse.</param>
    /// <param name="atomicType">If the parsed successfully, then the resulting <see cref="AtomicType"/> value;
    /// Otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the string input was parsed as an atomic type; Otherwise, <c>false</c>.</returns>
    public static bool TryParse(string value, out AtomicType? atomicType)
    {
        if (value.IsEquivalent("true") || value.IsEquivalent("false"))
        {
            atomicType = new BOOL(bool.Parse(value));
            return true;
        }

        if (Radix.TryInfer(value, out var radix) && radix is not null)
        {
            atomicType = radix.Parse(value);
            return true;
        }
        
        atomicType = default;
        return false;
    }

    /// <summary>
    /// Returns indication to whether the provided type name is the name of an atomic type.
    /// </summary>
    /// <param name="name">The type name to test.</param>
    /// <returns><c>true</c> if <c>name</c> is the name of any atomic type; otherwise, <c>false</c>.</returns>
    public static bool IsAtomic(string name) => Atomics.ContainsKey(name);
}