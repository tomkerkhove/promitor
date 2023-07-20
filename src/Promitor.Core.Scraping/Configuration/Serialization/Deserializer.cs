using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using GuardNet;
using Microsoft.Extensions.Logging;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    public abstract class Deserializer<TObject> : IDeserializer<TObject>
        where TObject: new()
    {
        private readonly HashSet<string> _ignoredFields = new HashSet<string>();
        private readonly List<IFieldDeserializationInfoBuilder> _fieldBuilders = new List<IFieldDeserializationInfoBuilder>();

        protected ILogger Logger { get; }

        protected Deserializer(ILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));

            Logger = logger;
        }

        /// <inheritdoc />
        public virtual TObject Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var result = new TObject();
            var fields = _fieldBuilders.Select(builder => builder.Build()).ToList();
            var deserializationContext = new DeserializationContext<TObject>(_ignoredFields, fields);

            foreach (var child in node.Children)
            {
                if (deserializationContext.IsIgnored(child.Key.ToString()))
                {
                    continue;
                }

                if (deserializationContext.TryGetField(child.Key.ToString(), out var fieldContext))
                {
                    fieldContext.SetValue(
                        result, GetFieldValue(child, fieldContext.DeserializationInfo, errorReporter));
                    fieldContext.Validate(child, errorReporter);
                }
                else
                {
                    var warningMessage = GetUnknownFieldWarningMessage(deserializationContext, child.Key.ToString());
                    errorReporter.ReportWarning(child.Key, warningMessage);
                }
            }

            foreach (var unsetField in deserializationContext.UnsetFields)
            {
                if (unsetField.DeserializationInfo.IsRequired)
                {
                    errorReporter.ReportError(node, $"'{unsetField.DeserializationInfo.YamlFieldName}' is a required field but was not found.");
                }
                else
                {
                    unsetField.SetDefaultValue(result);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<TObject> Deserialize(YamlSequenceNode nodes, IErrorReporter errorReporter)
        {
            Guard.NotNull(nodes, nameof(nodes));

            var deserializedObjects = new List<TObject>();
            foreach (var item in nodes)
            {
                if (!(item is YamlMappingNode metricNode))
                {
                    throw new SerializationException($"Failed parsing metrics because we couldn't cast an item to {nameof(YamlMappingNode)}");
                }

                var deserializedObject = Deserialize(metricNode, errorReporter);
                deserializedObjects.Add(deserializedObject);
            }

            return deserializedObjects;
        }

        public object DeserializeObject(YamlMappingNode node, IErrorReporter errorReporter)
        {
            return Deserialize(node, errorReporter);
        }

        protected FieldDeserializationInfoBuilder<TObject, TReturn> Map<TReturn>(Expression<Func<TObject, TReturn>> accessorExpression)
        {
            var builder = new FieldDeserializationInfoBuilder<TObject, TReturn>();
            builder.SetProperty(accessorExpression);

            _fieldBuilders.Add(builder);

            return builder;
        }

        protected void IgnoreField(string fieldName)
        {
            _ignoredFields.Add(fieldName);
        }

        private static object GetFieldValue(
            KeyValuePair<YamlNode, YamlNode> fieldNodePair, FieldDeserializationInfo fieldDeserializationInfo, IErrorReporter errorReporter)
        {
            if (fieldDeserializationInfo.CustomMapperFunc != null)
            {
                return fieldDeserializationInfo.CustomMapperFunc(fieldNodePair.Value.ToString(), fieldNodePair, errorReporter);
            }

            if (fieldDeserializationInfo.Deserializer != null)
            {
                return fieldNodePair.Value switch
                {
                    YamlMappingNode node => fieldDeserializationInfo.Deserializer.Deserialize(node, errorReporter),
                    YamlSequenceNode node => fieldDeserializationInfo.Deserializer.Deserialize(node, errorReporter),
                    _ => null
                };
            }

            var propertyType = Nullable.GetUnderlyingType(fieldDeserializationInfo.PropertyInfo.PropertyType) ?? fieldDeserializationInfo.PropertyInfo.PropertyType;
            if (propertyType.IsEnum)
            {
                var parseSucceeded = Enum.TryParse(propertyType, fieldNodePair.Value.ToString(), out var enumValue);
                if (!parseSucceeded)
                {
                    errorReporter.ReportError(fieldNodePair.Value, $"'{fieldNodePair.Value}' is not a valid value for '{fieldNodePair.Key}'.");
                }

                return enumValue;
            }
            
            if (typeof(IDictionary<string, string>).IsAssignableFrom(propertyType))
            {
                return ((YamlMappingNode)fieldNodePair.Value).GetDictionary();
            }

            if (propertyType == typeof(TimeSpan))
            {
                var parseSucceeded = TimeSpan.TryParse(fieldNodePair.Value.ToString(), out var timeSpanValue);
                if (!parseSucceeded)
                {
                    errorReporter.ReportError(fieldNodePair.Value, $"'{fieldNodePair.Value}' is not a valid value for '{fieldNodePair.Key}'. The value must be in the format 'hh:mm:ss'.");
                }

                return timeSpanValue;
            }

            try
            {
                return Convert.ChangeType(fieldNodePair.Value.ToString(), fieldDeserializationInfo.PropertyInfo.PropertyType);
            }
            catch (FormatException)
            {
                errorReporter.ReportError(fieldNodePair.Value, $"'{fieldNodePair.Value}' is not a valid value for '{fieldNodePair.Key}'. The value must be of type {fieldDeserializationInfo.PropertyInfo.PropertyType}.");
            }

            return null;
        }
        
        /// <summary>
        /// Gets the warning message to use when an invalid field name is found
        /// in the configuration.
        /// </summary>
        /// <param name="deserializationContext">The deserialization context.</param>
        /// <param name="fieldName">The unknown field.</param>
        private static string GetUnknownFieldWarningMessage(DeserializationContext<TObject> deserializationContext, string fieldName)
        {
            var message = $"Unknown field '{fieldName}'.";
            var suggestions = deserializationContext
                .GetSuggestions(fieldName)
                .Select(suggestion => $"'{suggestion}'")
                .ToList();

            if (suggestions.Any())
            {
                var formattedSuggestions = string.Join(", ", suggestions);
                message += $" Did you mean {formattedSuggestions}?";
            }

            return message;
        }
    }
}