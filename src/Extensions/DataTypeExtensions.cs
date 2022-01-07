﻿using System;
using System.Collections.Generic;
using System.Linq;
using L5Sharp.Comparers;
using L5Sharp.Core;

namespace L5Sharp.Extensions
{
    /// <summary>
    /// A static class containing extension methods for <see cref="IDataType"/>.
    /// </summary>
    public static class DataTypeExtensions
    {
        /// <summary>
        /// Determines if a member with the provided <see cref="TagName"/> exists as a descendant member of the
        /// current <see cref="IDataType"/>.
        /// </summary>
        /// <param name="dataType">The current <see cref="IDataType"/> instance.</param>
        /// <param name="tagName">The <see cref="TagName"/> of the descendant member to find.</param>
        /// <returns>true if a <see cref="IMember{TDataType}"/> with the specified name is contained in the nested
        /// hierarchy of the current <see cref="IDataType"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">tagName is null.</exception>
        /// <remarks>
        /// This method will recursively traverse the nested hierarchy of data type members to find all tag names.
        /// </remarks>
        public static bool ContainsMember(this IDataType dataType, TagName tagName)
        {
            if (tagName is null)
                throw new ArgumentNullException(nameof(tagName));

            var members = dataType.GetMembers().ToList();

            foreach (var memberName in tagName.Members)
            {
                var member = members.Find(m => m.Name == memberName);

                if (member is null)
                    return false;

                members = member.DataType.GetMembers().ToList();
            }

            return true;
        }

        /// <summary>
        /// Determines if two <see cref="IDataType"/> instances have an equal structure.
        /// </summary>
        /// <param name="dataType">The current <see cref="IDataType"/> instance.</param>
        /// <param name="other">The other <see cref="IDataType"/> to compare.</param>
        /// <returns>true if both <see cref="IDataType"/> objects have equal structure; otherwise, false.</returns>
        /// <remarks>
        /// An equivalent structure means both <see cref="IDataType"/> instances have equal names and all descendent members
        /// have equal name, data type, and dimensions. Each descendent member will in turn call
        /// <see cref="StructureEquals"/> on it's own type. This means that the entire hierarchical type structure is
        /// compared for equality. 
        /// </remarks>
        public static bool StructureEquals(this IDataType dataType, IDataType other)
        {
            var comparer = DataTypeStructureComparer.Instance;
            return comparer.Equals(dataType, other);
        } 

        /// <summary>
        /// Gets a <see cref="IMember{TDataType}"/> with the specified member name from the <see cref="IDataType"/>
        /// if it exists.
        /// </summary>
        /// <param name="dataType">The current <see cref="IDataType"/> instance.</param>
        /// <param name="name">The name of the <see cref="IMember{TDataType}"/> that is a child of the current type.</param>
        /// <returns>A <see cref="IMember{TDataType}"/> instance that represents the child</returns>
        public static IMember<IDataType>? GetMember(this IDataType dataType, string name) =>
            dataType.GetMembers().FirstOrDefault(m => m.Name == name);

        /// <summary>
        /// Gets the collection of immediate child members for the current <see cref="IDataType"/> if any exist.
        /// </summary>
        /// <param name="dataType">The current <see cref="IDataType"/> instance.</param>
        /// <returns>A collection of <see cref="IMember{TDataType}"/> objects that represent the immediate child
        /// members of the type.</returns>
        public static IEnumerable<IMember<IDataType>> GetMembers(this IDataType dataType)
        {
            return dataType switch
            {
                IComplexType complexType => complexType.Members,
                IArrayType<IDataType> arrayType => arrayType.AsEnumerable(),
                _ => Enumerable.Empty<IMember<IDataType>>()
            };
        }

        /// <summary>
        /// Gets all nested <see cref="IMember{TDataType}"/> objects up and to the final member name specified
        /// by the provided <see cref="TagName"/> value. 
        /// </summary>
        /// <param name="dataType">The current <see cref="IDataType"/> instance.</param>
        /// <param name="tagName">The full tag name for which to retrieve all nested members for.</param>
        /// <returns>
        /// A sequence of <see cref="IMember{TDataType}"/> that represent all the members specified in the provided tag name value.
        /// </returns>
        /// <exception cref="ArgumentNullException">tagName is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// The path defined by the provided tag name is not valid as a hierarchical path to a descendent member
        /// of the <see cref="IDataType"/>.
        /// </exception>
        /// <remarks>
        /// This method will recursively traverse the nested hierarchy of data type members to find all tag names.
        /// </remarks>
        public static IEnumerable<IMember<IDataType>> GetMembersTo(this IDataType dataType, TagName tagName)
        {
            if (string.IsNullOrEmpty(tagName))
                throw new ArgumentException("TagName can not be null or empty.");

            var results = new List<IMember<IDataType>>();

            var members = dataType.GetMembers().ToList();

            foreach (var memberName in tagName.Members)
            {
                var member = members.Find(m => m.Name == memberName);

                if (member is null) break;

                results.Add(member);
                members = member.DataType.GetMembers().ToList();
            }

            if (results.Count != tagName.Members.Count())
                throw new KeyNotFoundException(
                    $"The member '{tagName}' does not exist as a valid descendent of '{dataType.GetType()}'");

            return results;
        }

        /// <summary>
        /// Gets a collection of <see cref="TagName"/> values that represent all descendant member names of the <see cref="IDataType"/>.
        /// </summary>
        /// <param name="dataType">The current <see cref="IDataType"/> instance.</param>
        /// <returns>A collection of <see cref="TagName"/> values that represent all the descendent members of the
        /// current type.</returns>
        /// <remarks>
        /// This method will recursively traverse the nested hierarchy of data type members to find all tag names.
        /// </remarks>
        public static IEnumerable<TagName> GetTagNames(this IDataType dataType)
        {
            var names = new List<TagName>();

            foreach (var member in dataType.GetMembers())
            {
                names.Add(member.Name);
                names.AddRange(member.DataType.GetTagNames().Select(m => TagName.Combine(member.Name, m)));
            }

            return names;
        }

        /// <summary>
        /// Gets a collection of unique <see cref="IDataType"/> instances that represent all the data types the
        /// current type is dependent on. 
        /// </summary>
        /// <param name="dataType">The current <see cref="IDataType"/> instance.</param>
        /// <returns>
        /// A unique collection of <see cref="IDataType"/> instances if any dependents are found;
        /// otherwise; an empty collection.
        /// </returns>
        /// <remarks>
        /// This method will recursively traverse the nested hierarchy of data type members to find all tag names.
        /// </remarks>
        public static IEnumerable<IDataType> GetDependentTypes(this IDataType dataType)
        {
            var set = new HashSet<IDataType>(ComponentNameComparer.Instance);

            foreach (var member in dataType.GetMembers())
            {
                set.Add(member.DataType);
                set.UnionWith(member.DataType.GetDependentTypes());
            }

            return set;
        }
    }
}