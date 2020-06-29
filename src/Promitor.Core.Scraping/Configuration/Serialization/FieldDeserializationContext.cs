using System.Collections.Generic;
using GuardNet;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    /// <summary>
    /// Used to store information about a field while it is being deserialized.
    /// This object is not designed to be reused when deserializing multiple Yaml
    /// nodes, and a new copy should be created each time an object is deserialized.
    /// </summary>
    public class FieldDeserializationContext<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDeserializationContext{TObject}"/> type.
        /// </summary>
        /// <param name="deserializationInfo">The information about the field to deserialize.</param>
        public FieldDeserializationContext(FieldDeserializationInfo deserializationInfo)
        {
            Guard.NotNull(deserializationInfo, nameof(deserializationInfo));

            DeserializationInfo = deserializationInfo;
        }

        /// <summary>
        /// Gets the information about the field to deserialize.
        /// </summary>
        public FieldDeserializationInfo DeserializationInfo { get; }

        /// <summary>
        /// Gets a value indicating whether the field has been set.
        /// </summary>
        public bool HasBeenSet { get; private set; }

        /// <summary>
        /// Sets the field on the specified target object.
        /// </summary>
        /// <param name="target">The object being deserialized.</param>
        /// <param name="value">The value of the field.</param>
        public void SetValue(TObject target, object value)
        {
            DeserializationInfo.PropertyInfo.SetValue(target, value);
            HasBeenSet = true;
        }

        /// <summary>
        /// Runs any custom validators that have been defined for the field.
        /// </summary>
        /// <param name="fieldNodes">The pair of nodes defining the name of the field, and its value.</param>
        /// <param name="errorReporter">Used to report any validation errors.</param>
        public void Validate(KeyValuePair<YamlNode, YamlNode> fieldNodes, IErrorReporter errorReporter)
        {
            foreach (var validator in DeserializationInfo.Validators)
            {
                validator.Validate(fieldNodes.Value.ToString(), fieldNodes, errorReporter);
            }
        }

        /// <summary>
        /// Sets the field to its default value.
        /// </summary>
        /// <param name="target">The object being deserialized.</param>
        public void SetDefaultValue(TObject target)
        {
            DeserializationInfo.PropertyInfo.SetValue(target, DeserializationInfo.DefaultValue);
        }
    }
}
