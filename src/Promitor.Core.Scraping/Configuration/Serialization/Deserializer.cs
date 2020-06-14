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
                }
                else
                {
                    errorReporter.ReportWarning(child.Key, $"Unknown field '{child.Key}'.");
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

        protected void MapRequired<TReturn>(Expression<Func<TObject, TReturn>> accessorExpression, Func<string, KeyValuePair<YamlNode, YamlNode>, IErrorReporter, object> customMapperFunc = null)
        {
            var builder = new FieldDeserializationInfoBuilder<TObject, TReturn>();
            builder.SetProperty(accessorExpression);
            builder.IsRequired();
            builder.MapUsing(customMapperFunc);

            _fieldBuilders.Add(builder);
        }

        protected void MapRequired<TReturn>(
            Expression<Func<TObject, TReturn>> accessorExpression, IDeserializer deserializer)
            where TReturn: new()
        {
            var builder = new FieldDeserializationInfoBuilder<TObject, TReturn>();
            builder.SetProperty(accessorExpression);
            builder.IsRequired();
            builder.MapUsingDeserializer(deserializer);

            _fieldBuilders.Add(builder);
        }
        
        protected void MapOptional<TReturn>(
            Expression<Func<TObject, TReturn>> accessorExpression, TReturn defaultValue = default, Func<string, KeyValuePair<YamlNode, YamlNode>, IErrorReporter, object> customMapperFunc = null)
        {
            var builder = new FieldDeserializationInfoBuilder<TObject, TReturn>();
            builder.SetProperty(accessorExpression);
            builder.WithDefault(defaultValue);
            builder.MapUsing(customMapperFunc);

            _fieldBuilders.Add(builder);
        }

        protected void MapOptional<TReturn>(
            Expression<Func<TObject, TReturn>> accessorExpression, IDeserializer deserializer)
            where TReturn: new()
        {
            var builder = new FieldDeserializationInfoBuilder<TObject, TReturn>();
            builder.SetProperty(accessorExpression);
            builder.MapUsingDeserializer(deserializer);

            _fieldBuilders.Add(builder);
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
                return fieldDeserializationInfo.Deserializer.DeserializeObject((YamlMappingNode)fieldNodePair.Value, errorReporter);
            }

            var propertyType = Nullable.GetUnderlyingType(fieldDeserializationInfo.PropertyInfo.PropertyType) ?? fieldDeserializationInfo.PropertyInfo.PropertyType;
            if (propertyType.IsEnum)
            {
                var parseSucceeded = System.Enum.TryParse(
                    propertyType, fieldNodePair.Value.ToString(), out var enumValue);

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
    }
}