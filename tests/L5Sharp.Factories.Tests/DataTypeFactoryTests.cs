﻿using System;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;

namespace L5Sharp.Factories.Tests
{
    [TestFixture]
    public class DataTypeFactoryTests
    {
        private readonly string _fileName = Path.Combine(Environment.CurrentDirectory, @"TestFiles\Test.xml");
        private XDocument _document;
        private UserDefinedFactory _factory;

        [SetUp]
        public void Setup()
        {
            _document = XDocument.Load(_fileName);
            
            _factory = new UserDefinedFactory(null);
        }
        
    }
}