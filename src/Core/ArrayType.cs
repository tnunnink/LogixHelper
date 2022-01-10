﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using L5Sharp.Enums;

namespace L5Sharp.Core
{
    /// <inheritdoc />
    public class ArrayType<TDataType> : IArrayType<TDataType> where TDataType : IDataType
    {
        private readonly Dictionary<string, IMember<TDataType>> _elements;

        private TDataType SeedType => _elements.First().Value.DataType;

        /// <summary>
        /// Creates a new <see cref="IArrayType{TDataType}"/> with the provided dimensions and seed type.
        /// </summary>
        /// <param name="dimensions">The <see cref="Core.Dimensions"/> that define the array.</param>
        /// <param name="dataType">
        /// An optional <see cref="IDataType"/> instance used to initialize the collection.
        /// If the specified <see cref="TDataType"/> is not abstract and has a default parameterless constructor,
        /// <see cref="IArrayType{TDataType}"/> can construct an instance internally, which means you don't need
        /// to provide it.
        /// </param>
        /// <param name="radix"></param>
        /// <param name="externalAccess"></param>
        /// <param name="description"></param>
        /// <exception cref="ArgumentNullException">dimensions are null.</exception>
        /// <exception cref="ArgumentException">dimensions are empty.</exception>
        public ArrayType(Dimensions dimensions, TDataType? dataType = default,
            Radix? radix = null, ExternalAccess? externalAccess = null, string? description = null)
        {
            if (dimensions is null)
                throw new ArgumentNullException(nameof(dimensions));

            if (dimensions.AreEmpty)
                throw new ArgumentException("The provided dimensions can not be empty.");

            Dimensions = dimensions;

            dataType ??= CreateType();

            _elements = dimensions.CreateMembers(dataType, radix, externalAccess, description)
                .ToDictionary(m => m.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="dataTypes"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public ArrayType(Dimensions dimensions, IEnumerable<TDataType> dataTypes)
        {
            if (dimensions is null)
                throw new ArgumentNullException(nameof(dimensions));

            if (dimensions.AreEmpty)
                throw new ArgumentException("The provided dimensions can not be empty.");

            Dimensions = dimensions;

            if (dataTypes is null)
                throw new ArgumentNullException(nameof(dataTypes));

            _elements = dimensions.CreateMembers(dataTypes).ToDictionary(x => x.Name);
        }

        /// <inheritdoc />
        public string Name => $"{SeedType.Name}{Dimensions.ToBracketNotation()}";

        /// <inheritdoc />
        public string Description => string.Empty;

        /// <inheritdoc />
        public DataTypeFamily Family => SeedType.Family;

        /// <inheritdoc />
        public DataTypeClass Class => SeedType.Class;

        /// <inheritdoc />
        public Dimensions Dimensions { get; }

        /// <inheritdoc />
        public IMember<TDataType> this[int x] => Dimensions.DegreesOfFreedom == 1
            ? GetElement($"[{x}]")
            : throw new InvalidOperationException("The current array is not a one-dimensional array.");

        /// <inheritdoc />
        public IMember<TDataType> this[int x, int y] => Dimensions.DegreesOfFreedom == 2
            ? GetElement($"[{x},{y}]")
            : throw new InvalidOperationException("The current array is not a two-dimensional array.");

        /// <inheritdoc />
        public IMember<TDataType> this[int x, int y, int z] => Dimensions.DegreesOfFreedom == 3
            ? GetElement($"[{x},{y},{z}]")
            : throw new InvalidOperationException("The current array is not a three-dimensional array.");

        /// <inheritdoc />
        public IDataType Instantiate()
            => new ArrayType<TDataType>(Dimensions.Copy(), (TDataType)_elements.First().Value.DataType.Instantiate());

        /// <inheritdoc />
        public IEnumerator<IMember<TDataType>> GetEnumerator() => _elements.Values.GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets a <see cref="IMember{TDataType}"/> element with the provided index name if it exists.
        /// </summary>
        /// <param name="index">The string index value of the member element.</param>
        /// <returns>A <see cref="IMember{TDataType}"/> that represents the specified element of the array.</returns>
        /// <exception cref="ArgumentException">The provided index string is not a valid index for the array.</exception>
        private IMember<TDataType> GetElement(string index)
        {
            return _elements.ContainsKey(index)
                ? _elements[index]
                : throw new ArgumentOutOfRangeException(nameof(index),
                    $"The provided index '{index}' is outside the bounds of the array.");
        }

        /// <summary>
        /// Creates an instance of the specified data type.
        /// </summary>
        /// <returns>A new <see cref="TDataType"/> instance.</returns>
        /// <exception cref="ArgumentException">
        /// <see cref="TDataType"/> is abstract or does not have default parameterless constructor.
        /// </exception>
        private static TDataType CreateType()
        {
            var type = typeof(TDataType);

            if (type.IsAbstract)
                throw new ArgumentException(
                    $"The specified type '{typeof(TDataType)} is abstract. Abstract types can not be instantiated.");

            if (type.GetConstructor(Type.EmptyTypes) is null)
                throw new ArgumentException(
                    $"The specified type '{typeof(TDataType)} does not have a default parameterless constructor.");

            return Activator.CreateInstance<TDataType>();
        }
    }
}