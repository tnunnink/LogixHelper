﻿using System;
using System.Linq;
using System.Xml.Linq;
using L5Sharp.Core;
using L5Sharp.Enums;
using L5Sharp.Extensions;
using L5Sharp.L5X;
using L5Sharp.Predefined;
using L5Sharp.Serialization.Data;

namespace L5Sharp.Serialization.Components
{
    internal class TagSerializer : IL5XSerializer<ITag<IDataType>>
    {
        private static readonly XName ElementName = L5XElement.Tag.ToString();
        private readonly Func<string, IDataType> _dataTypeCreator;
        private readonly CommentSerializer _commentSerializer;
        private readonly FormattedDataSerializer _formattedDataSerializer;

        public TagSerializer()
        {
            _dataTypeCreator = DataType.Create;
            _commentSerializer = new CommentSerializer();
            _formattedDataSerializer = new FormattedDataSerializer();
        }

        public TagSerializer(L5XContext context)
        {
            _dataTypeCreator = context.Index.GetDataType;
            _commentSerializer = new CommentSerializer();
            _formattedDataSerializer = new FormattedDataSerializer();
        }

        public XElement Serialize(ITag<IDataType> component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            var element = new XElement(ElementName);

            element.Add(new XAttribute(L5XAttribute.Name.ToString(), component.Name));
            element.Add(new XElement(L5XElement.Description.ToString(), new XCData(component.Description)));
            element.Add(new XAttribute(L5XAttribute.TagType.ToString(), component.TagType));
            if (!component.Alias.IsEmpty)
                element.Add(new XAttribute(L5XAttribute.AliasFor.ToString(), component.Alias));
            element.Add(new XAttribute(L5XAttribute.DataType.ToString(), component.DataType));
            if (!component.Dimensions.IsEmpty)
                element.Add(new XAttribute(L5XAttribute.Dimensions.ToString(), component.Dimensions));
            if (!component.IsValueMember)
                element.Add(new XAttribute(L5XAttribute.Radix.ToString(), component.Radix));
            element.Add(new XAttribute(L5XAttribute.Constant.ToString(), component.Constant));
            element.Add(new XAttribute(L5XAttribute.ExternalAccess.ToString(), component.ExternalAccess));

            if (component.Comments.Any())
            {
                element.Add(_commentSerializer.Serialize(component.Comments));    
            }
            
            //todo same with engineering units and perhaps ranges min/max
            
            
            var data = _formattedDataSerializer.Serialize(component.DataType);
            element.Add(data);

            return element;
        }

        public ITag<IDataType> Deserialize(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (element.Name != ElementName)
                throw new ArgumentException($"Element '{element.Name}' not valid for the serializer {GetType()}.");

            var name = element.ComponentName();
            var description = element.ComponentDescription();
            var dataType = _dataTypeCreator.Invoke(element.DataTypeName());
            
            var dimensions = element.Attribute(L5XAttribute.Dimensions.ToString())?.Value.Parse<Dimensions>();
            var radix = element.Attribute(L5XAttribute.Radix.ToString())?.Value.Parse<Radix>();
            var access = element.Attribute(L5XAttribute.ExternalAccess.ToString())?.Value.Parse<ExternalAccess>();
            var usage = element.Attribute(L5XAttribute.Usage.ToString())?.Value.Parse<TagUsage>();
            var alias = element.Attribute(L5XAttribute.AliasFor.ToString())?.Value.Parse<TagName>();
            var constant = element.Attribute(L5XAttribute.Constant.ToString())?.Value.Parse<bool>() ?? default;
            
            var commentsElement = element.Elements(L5XElement.Comments.ToString()).FirstOrDefault();
            var comments = commentsElement is not null ? _commentSerializer.Deserialize(commentsElement) : null; 
                
            var type = dimensions is not null && dimensions.IsEmpty
                ? new ArrayType<IDataType>(dimensions, dataType, radix, access, description)
                : dataType;

            var tag = new Tag<IDataType>(name, type, radix, access, description, usage, alias, constant, comments);

            var formattedData = element.Descendants(L5XElement.Data.ToString())
                .FirstOrDefault(e => e.Attribute(L5XAttribute.Format.ToString())?.Value != TagDataFormat.L5K);

            if (formattedData is null) return tag;
            
            var data = _formattedDataSerializer.Deserialize(formattedData);
            tag.SetData(data);
            return tag;
        }
    }
}