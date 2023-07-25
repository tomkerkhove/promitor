using System;
using System.Collections.Generic;
using System.Reflection;
using Promitor.Core.Scraping.Configuration.Serialization.FieldValidators;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    /// <summary>
    /// Contains information and helper methods needed to deserialize a particular field.
    /// This class should be immutable, and should be able to be reused when deserializing
    /// multiple Yaml nodes.
    /// </summary>
    public class FieldDeserializationInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDeserializationInfo"/> type.
        /// </summary>
        /// <param name="propertyInfo">The property being deserialized.</param>
        /// <param name="isRequired">Whether the field is required or not.</param>
        /// <param name="defaultValue">The default value to use for the field if not specified.</param>
        /// <param name="customMapperFunc">A custom function to use for getting the value of the field.</param>
        /// <param name="deserializer">A deserializer to use to deserialize the field.</param>
        /// <param name="validators">Any custom validators to validate the field using.</param>
        public FieldDeserializationInfo(
            PropertyInfo propertyInfo,
            bool isRequired,
            object defaultValue,
            Func<string, KeyValuePair<YamlNode, YamlNode>, IErrorReporter, object> customMapperFunc,
            IDeserializer<object> deserializer,
            IReadOnlyCollection<IFieldValidator> validators)
        {
            YamlFieldName = GetName(propertyInfo);
            CustomMapperFunc = customMapperFunc;
            PropertyInfo = propertyInfo;
            IsRequired = isRequired;
            DefaultValue = defaultValue;
            Deserializer = deserializer;
            Validators = validators;
        }

        /// <summary>
        /// Gets the Yaml field name.
        /// </summary>
        public string YamlFieldName { get; }

        /// <summary>
        /// Gets information about the property that the field should be deserialized into.
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets a value indicating whether the field is required or not.
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        /// Gets the default value that should be used for the field if none is supplied.
        /// </summary>
        public object DefaultValue { get; }

        /// <summary>
        /// Gets a custom function that can be used to deserialize the field.
        /// </summary>
        public Func<string, KeyValuePair<YamlNode, YamlNode>, IErrorReporter, object> CustomMapperFunc { get; }

        /// <summary>
        /// Gets a deserializer to use when deserializing the field.
        /// </summary>
        public IDeserializer<object> Deserializer { get; }

        /// <summary>
        /// Gets the custom validators for the field.
        /// </summary>
        public IReadOnlyCollection<IFieldValidator> Validators { get; }

        private static string GetName(MemberInfo propertyInfo)
        {
            return char.ToLowerInvariant(propertyInfo.Name[0]) + propertyInfo.Name.Substring(1);
        }
    }
}