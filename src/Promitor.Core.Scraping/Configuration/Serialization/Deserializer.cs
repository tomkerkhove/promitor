using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        private readonly Dictionary<string, FieldContext> _fields = new Dictionary<string, FieldContext>();

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

            foreach (var child in node.Children)
            {
                if (_ignoredFields.Contains(child.Key.ToString()))
                {
                    continue;
                }

                if (_fields.TryGetValue(child.Key.ToString(), out var fieldContext))
                {
                    fieldContext.SetValue(
                        result, GetFieldValue(child, fieldContext, errorReporter));
                }
                else
                {
                    errorReporter.ReportWarning(child.Key, $"Unknown field '{child.Key}'.");
                }
            }

            foreach (var unsetField in _fields.Where(field => !field.Value.HasBeenSet))
            {
                if (unsetField.Value.IsRequired)
                {
                    errorReporter.ReportError(node, $"'{unsetField.Key}' is a required field but was not found.");
                }
                else
                {
                    unsetField.Value.SetDefaultValue(result);
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
            var memberExpression = (MemberExpression)accessorExpression.Body;
            _fields[GetName(memberExpression)] = new FieldContext(memberExpression.Member as PropertyInfo, true, default(TReturn), customMapperFunc);
        }

        protected void MapRequired<TReturn>(
            Expression<Func<TObject, TReturn>> accessorExpression, IDeserializer deserializer)
            where TReturn: new()
        {
            var memberExpression = (MemberExpression)accessorExpression.Body;
            _fields[GetName(memberExpression)] = new FieldContext(
                memberExpression.Member as PropertyInfo, true, default(TReturn), null, deserializer);
        }
        
        protected void MapOptional<TReturn>(
            Expression<Func<TObject, TReturn>> accessorExpression, TReturn defaultValue = default, Func<string, KeyValuePair<YamlNode, YamlNode>, IErrorReporter, object> customMapperFunc = null)
        {
            var memberExpression = (MemberExpression)accessorExpression.Body;
            _fields[GetName(memberExpression)] = new FieldContext(memberExpression.Member as PropertyInfo, false, defaultValue, customMapperFunc);
        }

        protected void MapOptional<TReturn>(
            Expression<Func<TObject, TReturn>> accessorExpression, IDeserializer deserializer)
            where TReturn: new()
        {
            var memberExpression = (MemberExpression)accessorExpression.Body;
            _fields[GetName(memberExpression)] = new FieldContext(
                memberExpression.Member as PropertyInfo, false, default(TReturn), null, deserializer);
        }

        protected void IgnoreField(string fieldName)
        {
            _ignoredFields.Add(fieldName);
        }

        private static string GetName(MemberExpression memberExpression)
        {
            return Char.ToLowerInvariant(memberExpression.Member.Name[0]) + memberExpression.Member.Name.Substring(1);
        }

        private static object GetFieldValue(
            KeyValuePair<YamlNode, YamlNode> fieldNodePair, FieldContext fieldContext, IErrorReporter errorReporter)
        {
            if (fieldContext.CustomMapperFunc != null)
            {
                return fieldContext.CustomMapperFunc(fieldNodePair.Value.ToString(), fieldNodePair, errorReporter);
            }


            if (fieldContext.Deserializer != null)
            {
                return fieldContext.Deserializer.DeserializeObject((YamlMappingNode)fieldNodePair.Value, errorReporter);
            }

            var propertyType = Nullable.GetUnderlyingType(fieldContext.PropertyInfo.PropertyType) ?? fieldContext.PropertyInfo.PropertyType;
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
                return Convert.ChangeType(fieldNodePair.Value.ToString(), fieldContext.PropertyInfo.PropertyType);
            }
            catch (FormatException)
            {
                errorReporter.ReportError(fieldNodePair.Value, $"'{fieldNodePair.Value}' is not a valid value for '{fieldNodePair.Key}'. The value must be of type {fieldContext.PropertyInfo.PropertyType}.");
            }

            return null;
        }

        private class FieldContext
        {
            public FieldContext(PropertyInfo propertyInfo, bool isRequired, object defaultValue, Func<string, KeyValuePair<YamlNode, YamlNode>, IErrorReporter, object> customMapperFunc, IDeserializer deserializer = null)
            {
                CustomMapperFunc = customMapperFunc;
                PropertyInfo = propertyInfo;
                IsRequired = isRequired;
                DefaultValue = defaultValue;
                Deserializer = deserializer;
            }

            public bool HasBeenSet { get; private set; }
            public PropertyInfo PropertyInfo { get; }
            public bool IsRequired { get; }
            public object DefaultValue { get; }
            public Func<string, KeyValuePair<YamlNode, YamlNode>, IErrorReporter, object> CustomMapperFunc { get; }
            public IDeserializer Deserializer { get; }

            public void SetValue(TObject target, object value)
            {
                PropertyInfo.SetValue(target, value);
                HasBeenSet = true;
            }

            public void SetDefaultValue(TObject target)
            {
                PropertyInfo.SetValue(target, DefaultValue);
            }
        }
    }
}