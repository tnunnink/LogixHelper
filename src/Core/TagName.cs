﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using L5Sharp.Extensions;
using L5Sharp.Utilities;

namespace L5Sharp.Core
{
    /// <summary>
    /// A value representing a tag name string. This value type class make working with string tag name easier..
    /// </summary>
    public class TagName : IEquatable<TagName>, IComparable<TagName>
    {
        private const string ArrayBracket = "[";
        private const string MemberSeparator = ".";
        private const string TagNamePattern = @"^[A-Za-z_][\w\.[\],:]+$";
        private const string MemberPattern = @"^[A-Za-z_][\w:]+$|^\[\d+\]$|^\[\d+,\d+\]$|^\[\d+,\d+,\d+\]$";
        private const string MembersPattern = @"[\w:]+|\[[\d,]+\]";
        private const string PartsPattern = @"\w+|\[[\d,]+\]";
        private readonly string _tagName;

        /// <summary>
        /// Creates a new <see cref="TagName"/> object with the provided string tag name.
        /// </summary>
        /// <param name="tagName">The string that represents the tag name value.</param>
        /// <exception cref="ArgumentNullException">tagName is null.</exception>
        public TagName(string tagName)
        {
            _tagName = tagName ?? throw new ArgumentNullException(nameof(tagName));
        }

        /// <summary>
        /// Gets the tag or root portion of the <see cref="TagName"/> string.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The root portion of a given tag name is simply the beginning part of the tag name up to the first
        /// member separator character ('.'). For Module defined tags, this includes the colon separator. This represent
        /// the actual user configured tag name. The remaining tag name string (if any) should the data type structure member.
        /// </para>
        /// <para>
        /// This value can be swapped out easily using <see cref="Rename"/> to return a new <see cref="TagName"/> with the
        /// newly specified root tag name value.
        /// </para>
        /// </remarks>
        public string Tag => _tagName[..GetRootLength()];

        /// <summary>
        /// Gets the operand portion of the <see cref="TagName"/> value.
        /// </summary>
        /// <remarks>
        /// The operand of a tag name represents the part of the name after the base name. This value will always be
        /// the full tag name value without the leading base name. The operand will include any leading '.' character.
        /// </remarks>
        /// <seealso cref="Path"/>
        public string Operand => !Tag.IsEmpty() ? _tagName.Remove(0, Tag.Length) : string.Empty;

        /// <summary>
        /// Gets the member path of the tag name value.
        /// </summary>
        /// <remarks>
        /// The path of a tag name represents a name relative to <see cref="Tag"/>. The value will always be the full tag name
        /// without the leading root name. This is similar to <see cref="Operand"/>, except that is also removes any
        /// leading member separator character ('.'). 
        /// </remarks>
        /// <seealso cref="Operand"/>
        public string Path => !Operand.IsEmpty() ? Operand.Remove(0, 1) : string.Empty;

        /// <summary>
        /// Gets the ending member name string of the tag name value.
        /// </summary>
        public string Member => Members.Last();

        /// <summary>
        /// A number representing the depth, or number of members from the root tag name, of the current tag name value.
        /// </summary>
        /// <remarks>
        /// This value represents the number of members between the root member and current member (i.e. one less than
        /// <see cref="Members"/> to exclude the base name). This is helpful for filtering tag descendents. Note that array
        /// indices are also considered a member. For example, 'MyTag[1].Value' has a depth of 2 since '[1]' and 'Value'
        /// are descendent member names of the root tag 'MyTag' member.
        /// </remarks>
        public int Depth => Members.Count() - 1;

        /// <summary>
        /// Gets the collection of member name strings that comprise the entire tag name value.
        /// </summary>
        /// <remarks>
        /// The members of a tag name represent all the individual constituent parts of the full name. This includes
        /// the index arrays if any exist. For Modules, the root is considered a single member.
        /// The root will include all character up to the first '.' member separator character.
        /// </remarks>
        public IEnumerable<string> Members =>
            Regex.Matches(_tagName, MembersPattern, RegexOptions.Compiled).Select(m => m.Value);

        /// <summary>
        /// Gets the each part of the tag name string separated by ':' or '.' characters.
        /// </summary>
        /// <remarks>
        /// This is similar to <see cref="Members"/>, except it splits the string by colons as well. If the tag name
        /// value is not a module tag, then <see cref="Parts"/> should be equivalent to <see cref="Members"/> 
        /// </remarks>
        public IEnumerable<string> Parts =>
            Regex.Matches(_tagName, PartsPattern, RegexOptions.Compiled).Select(m => m.Value);

        /// <summary>
        /// Gets a value indicating whether the current <see cref="TagName"/> value is empty.
        /// </summary>
        public bool IsEmpty => string.IsNullOrWhiteSpace(_tagName);

        /// <summary>
        /// Gets a value indicating whether the current <see cref="TagName"/> is a valid representation of a tag name.
        /// </summary>
        public bool IsValid => Regex.IsMatch(_tagName, TagNamePattern, RegexOptions.Compiled);

        /// <summary>
        /// Gets the static empty <see cref="TagName"/> value.
        /// </summary>
        public static TagName Empty => new(string.Empty);

        /// <summary>
        /// Combines a series of strings into a single <see cref="TagName"/> value, inserting member separator
        /// characters as needed.
        /// </summary>
        /// <param name="members">The series of strings that, in order, comprise the full tag name value.</param>
        /// <returns>A new <see cref="TagName"/>value that represents the combination of all provided member names.</returns>
        /// <exception cref="ArgumentException">If any provided member does not match the member pattern format.</exception>
        public static TagName Combine(params string[] members)
        {
            var builder = new StringBuilder();

            foreach (var name in members)
            {
                if (!Regex.IsMatch(name, MemberPattern))
                    throw new FormatException($"The provided name '{name}' is not a valid member name format.");

                //If the current member is not an array element or doesn't already have a member separator, add one.
                if (!(name.StartsWith(ArrayBracket) || !name.StartsWith(MemberSeparator)) && builder.Length > 1)
                    builder.Append(MemberSeparator);

                builder.Append(name);
            }

            return new TagName(builder.ToString());
        }

        /// <summary>
        /// Combines a collection of member names into a single <see cref="TagName"/> value.
        /// </summary>
        /// <param name="members">The collection of strings that represent the member names of the tag name value.</param>
        /// <returns>A new <see cref="TagName"/>A new <see cref="TagName"/> value that is the combination of all provided member names.</returns>
        /// <exception cref="ArgumentException">If a provided name does not match the member pattern format.</exception>
        public static TagName Combine(IEnumerable<string> members)
        {
            var builder = new StringBuilder();

            foreach (var name in members)
            {
                if (!Regex.IsMatch(name, MemberPattern))
                    throw new FormatException($"The provided name '{name}' is not a valid member name format.");

                //If the current member is not an array element or doesn't already have a member separator, add one.
                if (!(name.StartsWith(ArrayBracket) || !name.StartsWith(MemberSeparator)) && builder.Length > 1)
                    builder.Append(MemberSeparator);

                builder.Append(name);
            }

            return new TagName(builder.ToString());
        }

        /// <summary>
        /// Converts a <see cref="TagName"/> to a <see cref="string"/> value.
        /// </summary>
        /// <param name="tagName">The <see cref="TagName"/> value to convert.</param>
        /// <returns>A new <see cref="string"/> value representing the value of the tag name.</returns>
        public static implicit operator string(TagName tagName) => tagName is not null
            ? tagName._tagName
            : throw new ArgumentNullException(nameof(tagName));

        /// <summary>
        /// Converts a <see cref="string"/> to a <see cref="TagName"/> value.
        /// </summary>
        /// <param name="tagName">The <see cref="string"/> value to convert.</param>
        /// <returns>A new <see cref="TagName"/> value representing the value of the tag name.</returns>
        public static implicit operator TagName(string tagName) => new(tagName);

        /// <summary>
        /// Determines if the provided tagName is contained within the current value.
        /// </summary>
        /// <param name="tagName">The tag name to evaluate as a sub path or contained tag name path.</param>
        /// <returns>true if tagName is contained within the current value; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">tagName is null.</exception>
        public bool Contains(TagName tagName)
        {
            if (tagName is null)
                throw new ArgumentNullException(nameof(tagName));

            return _tagName.Contains(tagName);
        }

        /// <summary>
        /// Creates a copy of the current <see cref="TagName"/> object with the same value.
        /// </summary>
        /// <returns>A new <see cref="TagName"/> instance with the value of the current tag name.</returns>
        public TagName Copy() => new(string.Copy(_tagName));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public TagName Rename(string root) => Combine(root, Operand);

        /// <inheritdoc />
        public override string ToString() => _tagName;

        /// <summary>
        /// Determines whether the specified <see cref="TagName"/> objects are equal.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static bool Equals(TagName first, TagName second, IEqualityComparer<TagName> comparer)
        {
            return comparer.Equals(first, second);
        }

        /// <inheritdoc /> 
        public bool Equals(TagName? other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) ||
                   StringComparer.OrdinalIgnoreCase.Equals(_tagName, other._tagName);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => Equals(obj as TagName);

        /// <inheritdoc />
        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(_tagName);

        /// <summary>
        /// Determines if the provided objects are equal.
        /// </summary>
        /// <param name="left">An object to compare.</param>
        /// <param name="right">An object to compare.</param>
        /// <returns>true if the provided objects are equal; otherwise, false.</returns>
        public static bool operator ==(TagName? left, TagName? right) => Equals(left, right);

        /// <summary>
        /// Determines if the provided objects are not equal.
        /// </summary>
        /// <param name="left">An object to compare.</param>
        /// <param name="right">An object to compare.</param>
        /// <returns>true if the provided objects are not equal; otherwise, false.</returns>
        public static bool operator !=(TagName? left, TagName? right) => !Equals(left, right);

        /// <inheritdoc />
        public int CompareTo(TagName? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            return ReferenceEquals(null, other)
                ? 1
                : string.Compare(_tagName, other._tagName, StringComparison.OrdinalIgnoreCase);
        }

        private int GetRootLength()
        {
            var index = _tagName.IndexOf(MemberSeparator, StringComparison.Ordinal);
            return index >= 0 ? index : _tagName.Length;
        }
    }
}