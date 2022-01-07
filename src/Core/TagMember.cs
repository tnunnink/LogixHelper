﻿using System;
using System.Collections.Generic;
using System.Linq;
using L5Sharp.Enums;
using L5Sharp.Extensions;

namespace L5Sharp.Core
{
    /// <inheritdoc />
    public class TagMember<TDataType> : ITagMember<TDataType> where TDataType : IDataType
    {
        private readonly IMember<TDataType> _member;

        internal TagMember(IMember<TDataType> member, ITag<IDataType> root, ITagMember<IDataType>? parent)
        {
            _member = member ?? throw new ArgumentNullException(nameof(member));
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Parent = parent;
        }

        /// <inheritdoc />
        public string Name => _member.Name;

        /// <inheritdoc />
        public string Description => Root.Comments.Contains(TagName)
            ? Root.Comments.Get(TagName)
            : CalculateDescription();

        /// <inheritdoc />
        public TagName TagName => Parent is null
            ? new TagName(Name)
            : TagName.Combine(Parent.TagName, Name);

        /// <inheritdoc />
        public TDataType DataType => _member.DataType;

        /// <inheritdoc />
        public Dimensions Dimensions => _member.Dimensions;

        /// <inheritdoc />
        public Radix Radix => _member.Radix;

        /// <inheritdoc />
        public ExternalAccess ExternalAccess => Parent is null
            ? _member.ExternalAccess
            : ExternalAccess.MostRestrictive(_member.ExternalAccess, Parent.ExternalAccess);

        /// <inheritdoc />
        public bool IsValueMember => _member.IsValueMember;

        /// <inheritdoc />
        public bool IsStructureMember => _member.IsStructureMember;

        /// <inheritdoc />
        public bool IsArrayMember => _member.IsArrayMember;

        /// <inheritdoc />
        public object? Value => _member.DataType switch
        {
            IAtomicType atomic => atomic.Value,
            IStringType stringType => stringType.Value,
            _ => null
        };

        /// <inheritdoc />
        public ITagMember<IDataType>? Parent { get; }

        /// <inheritdoc />
        public ITag<IDataType> Root { get; }

        /// <inheritdoc />
        public ITagMember<IDataType> this[int x] => _member.DataType is IArrayType<IDataType> arrayType
            ? new TagMember<IDataType>(arrayType[x], Root, (ITagMember<IDataType>)this)
            : throw new InvalidOperationException(
                $"Tag of type '{GetType()}' is not a valid '{typeof(IArrayType<IDataType>)}'");

        /// <inheritdoc />
        public ITagMember<IDataType> this[int x, int y] => _member.DataType is IArrayType<IDataType> arrayType
            ? new TagMember<IDataType>(arrayType[x, y], Root, (ITagMember<IDataType>)this)
            : throw new InvalidOperationException();

        /// <inheritdoc />
        public ITagMember<IDataType> this[int x, int y, int z] => _member.DataType is IArrayType<IDataType> arrayType
            ? new TagMember<IDataType>(arrayType[x, y, z], Root, (ITagMember<IDataType>)this)
            : throw new InvalidOperationException();

        /// <inheritdoc />
        public ITagMember<IDataType> this[TagName tagName] => GetMemberInternal(tagName);

        /// <inheritdoc />
        public void Comment(string comment) => Root.Comments.Set(new Comment(TagName, comment));

        /// <inheritdoc />
        public bool Contains(TagName tagName) => GetTagNames().Contains(tagName);

        /// <inheritdoc />
        public ITagMember<TMemberType> GetMember<TMemberType>(Func<TDataType, IMember<TMemberType>> selector)
            where TMemberType : IDataType
        {
            var parent = (ITagMember<IDataType>)this;

            var member = selector.Invoke(_member.DataType);

            return new TagMember<TMemberType>(member, Root, parent);
        }

        /// <inheritdoc />
        public IEnumerable<ITagMember<IDataType>> GetMembers() =>
            _member.DataType.GetMembers().Select(m => new TagMember<IDataType>(m, Root, (ITagMember<IDataType>)this));

        /// <inheritdoc />
        public IEnumerable<TagName> GetTagNames() =>
            _member.DataType.GetTagNames().Select(n => TagName.Combine(TagName, n));

        /// <inheritdoc />
        public bool TryGetMember(TagName name, out ITagMember<IDataType>? tagMember)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool TrySetValue(IAtomicType? value)
        {
            if (value is null || value.GetType() != _member.DataType.GetType() ||
                _member.DataType is not IAtomicType atomicType) return false;

            atomicType.SetValue(value);

            return true;
        }

        /// <inheritdoc />
        public void SetValue(IAtomicType value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (_member.DataType is not IAtomicType atomicType)
                throw new InvalidOperationException(
                    $"Can not set value for '{GetType()}'. Type must be atomic.");

            atomicType.SetValue(value);
        }

        private ITagMember<IDataType> GetMemberInternal(TagName tagName)
        {
            //Allow both relative and absolute path from the current member to be used.
            var path = tagName.Base.Equals(Root.Name) ? tagName.Path : tagName.ToString();

            var members = _member.DataType.GetMembersTo(path);

            var parent = (ITagMember<IDataType>)this;

            foreach (var member in members)
            {
                var current = new TagMember<IDataType>(member, Root, parent);
                parent = current;
            }

            return parent;
        }

        /// <summary>
        /// Determines the value of the member description based on the rules of the "Pass-Through-Description".
        /// </summary>
        /// <returns>A string description for the current member.</returns>
        private string CalculateDescription()
        {
            if (!Root.DataType.Class.Equals(DataTypeClass.User))
                return Parent is not null ? Parent.Description : string.Empty;

            if (Parent is null)
                return _member.DataType.Description;

            return IsArrayMember
                ? $"{Root.Description} {_member.Description}".Trim()
                : $"{Root.Description} {Parent.Description}".Trim();
        }

        public IMember<TDataType> Copy()
        {
            throw new NotImplementedException();
        }
    }
}