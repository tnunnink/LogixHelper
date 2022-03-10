﻿using System;
using System.Linq;
using System.Xml.Linq;
using L5Sharp.L5X;
using L5Sharp.Repositories;

namespace L5Sharp
{
    /// <inheritdoc />
    public class L5XContext : ILogixContext
    {
        /// <summary>
        /// Creates a new <see cref="L5XContext"/> instance with the provided <see cref="L5XDocument"/>.
        /// </summary>
        /// <param name="l5X">The <see cref="L5XDocument"/> instance that represents the loaded L5X.</param>
        private L5XContext(L5XDocument l5X)
        {
            L5X = l5X ?? throw new ArgumentNullException(nameof(l5X));
            Serializers = new L5XSerializers(this);
            TypeIndex = new L5XTypeIndex(this);

            DataTypes = new DataTypeRepository(this);
            Modules = new ModuleRepository(this);
            Tags = new TagRepository(this);
            Programs = new ProgramRepository(this);
            Tasks = new TaskRepository(this);
        }

        /// <summary>
        /// Creates a new <see cref="L5XContext"/> by loading the contents of the provide file name.
        /// </summary>
        /// <param name="fileName">The full path, including file name, to the L5X file to load.</param>
        /// <returns>A new <see cref="L5XContext"/> containing the contents of the specified file.</returns>
        /// <exception cref="ArgumentException">The string is null or empty.</exception>
        /// <remarks>
        /// This factory method uses the <see cref="XDocument.Load(string)"/> to load the contents of the xml file into
        /// memory. This means that this method is subject to the same exceptions that could be generated by loading the
        /// XDocument. Once loaded, validation is performed to ensure the content adheres to the specified L5X Schema files.
        /// </remarks>
        public static L5XContext Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("Filename can not be null or empty");

            var document = new L5XDocument(XDocument.Load(fileName));

            return new L5XContext(document);
        }

        /// <summary>
        /// Creates a new <see cref="L5XContext"/> with the provided L5X string content.
        /// </summary>
        /// <param name="content">The string that contains the L5X content to parse.</param>
        /// <returns>A new <see cref="L5XContext"/> containing the contents of the specified string.</returns>
        /// <exception cref="ArgumentException">The string is null or empty.</exception>
        /// <remarks>
        /// This factory method uses the <see cref="XDocument.Parse(string)"/> to load the contents of the xml file into
        /// memory. This means that this method is subject to the same exceptions that could be generated by parsing the
        /// XDocument. Once parsed, validation is performed to ensure the content adheres to the specified L5X Schema files.
        /// </remarks>
        public static L5XContext Parse(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Filename can not be null or empty");

            var document = new L5XDocument(XDocument.Parse(content));

            return new L5XContext(document);
        }

        /// <summary>
        /// Creates a new <see cref="L5XContext"/> target for the provided component.
        /// </summary>
        /// <param name="component">The <see cref="ILogixComponent"/> instance for which to create the new context.</param>
        /// <typeparam name="TComponent">The logix component type.</typeparam>
        /// <returns>
        /// A new <see cref="L5XContext"/> instance for the provided logix component as the target of the new context.
        /// </returns>
        public static L5XContext Create<TComponent>(TComponent component)
            where TComponent : ILogixComponent => new(L5XDocument.Create(component));

        /// <summary>
        /// Gets the underlying <see cref="L5XDocument"/> that the current context represents. 
        /// </summary>
        public L5XDocument L5X { get; }

        /// <summary>
        /// 
        /// </summary>
        internal readonly L5XSerializers Serializers;

        /// <summary>
        /// 
        /// </summary>
        internal readonly L5XTypeIndex TypeIndex;

        /// <inheritdoc />
        public IController Controller => Serializers.GetSerializer<IController>()
            .Deserialize(L5X.GetComponents<IController>().First());

        /// <inheritdoc />
        public IRepository<IUserDefined> DataTypes { get; }

        /// <inheritdoc />
        public IModuleRepository Modules { get; }

        /// <inheritdoc />
        public IRepository<ITag<IDataType>> Tags { get; }

        /// <inheritdoc />
        public IRepository<IProgram> Programs { get; }

        /// <inheritdoc />
        public IReadOnlyRepository<ITask> Tasks { get; }

        /// <inheritdoc />
        public override string ToString() => L5X.Content.ToString();

        /// <summary>
        /// Saves the current content of the <see cref="L5XContext"/> to the specified file,
        /// overwriting an existing file, if it exists.
        /// </summary>
        /// <param name="fileName">The name of the file to save.</param>
        public void Save(string fileName)
        {
            L5X.Save(fileName);
        }
    }
}