﻿using System;
using System.Collections.Generic;
using System.Linq;
using L5Sharp.Abstractions;
using L5Sharp.Enums;
using L5Sharp.Utilities;

namespace L5Sharp.Core
{
    public class StringType : IDataType
    {
        private static readonly string[] MemberNames = { "LEN", "DATA" };
        private string _name;
        private string _description;

        private readonly Dictionary<string, Member> _members = new Dictionary<string, Member>();

        public StringType(string name, ushort length, string description = null)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            if (length <= 0)
                throw new ArgumentException("Length must be greater that 0");

            Name = name;
            Description = description ?? string.Empty;
            
            _members.Add(MemberNames[0],
                new Member(MemberNames[0], Logix.DataType.Dint));
            _members.Add(MemberNames[1],
                new Member(MemberNames[1], Logix.DataType.Sint, new Dimensions(length), Radix.Ascii));
        }

        public string Name
        {
            get => _name;
            set
            {
                Validate.Name(value);
                Validate.DataTypeName(value);
                _name = value;
            }
        }

        public DataTypeFamily Family => DataTypeFamily.String;

        public DataTypeClass Class => DataTypeClass.User;

        public bool IsAtomic => false;

        public object DefaultValue => string.Empty;

        public Radix DefaultRadix => Radix.Null;

        public TagDataFormat DataFormat => TagDataFormat.String;

        public string Description
        {
            get => _description;
            set => _description = value ?? string.Empty;
        }

        public IMember Len => Members.SingleOrDefault(x => x.Name == nameof(Len).ToUpper());
        public IMember Data => Members.SingleOrDefault(x => x.Name == nameof(Data).ToUpper());

        public IEnumerable<IMember> Members => _members.Values.AsEnumerable();

        public IMember GetMember(string name)
        {
            _members.TryGetValue(name, out var member);
            return member;
        }

        public IEnumerable<IDataType> GetDependentTypes()
        {
            return _members.Select(member => member.Value.DataType);
        }

        public void UpdateLength(ushort length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than 0");

            var data = new Member(MemberNames[1], Logix.DataType.Sint, new Dimensions(length), Radix.Ascii);
            
            _members.Remove(MemberNames[1]);
            _members.Add(data.Name, data);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}