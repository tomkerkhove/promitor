using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Promitor.Core.Scraping.Configuration.Serialization.FieldValidators;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    /// <summary>
    /// Used to build a <see cref="FieldDeserializationInfo" /> object.
    /// </summary>
    public class FieldDeserializationInfoBuilder<TObject, TReturn> : IFieldDeserializationInfoBuilder
    {
        private readonly List<IFieldValidator> _validators = new List<IFieldValidator>();
        private PropertyInfo _propertyInfo;
        private bool _isRequired;
        private object _defaultValue;
        private Func<string, KeyValuePair<YamlNode, YamlNode>, IErrorReporter, object> _customMapperFunc;
        private IDeserializer<object> _deserializer;

        /// <summary>
        /// Sets the expression that defines the property to map.
        /// </summary>
        /// <param name="accessorExpression">The expression used to map the property.</param>
        public void SetProperty(Expression<Func<TObject, TReturn>> accessorExpression)
        {
            var memberExpression = accessorExpression.Body as MemberExpression;

            if (memberExpression?.Member is PropertyInfo info)
            {
                _propertyInfo = info;
            }
            else
            {
                throw new ArgumentException("The expression must be a property accessor", nameof(accessorExpression));
            }
        }

        /// <summary>
        /// Marks the field as required.
        /// </summary>
        /// <returns>The builder.</returns>
        public FieldDeserializationInfoBuilder<TObject, TReturn> IsRequired()
        {
            _isRequired = true;

            return this;
        }

        /// <summary>
        /// Specifies a default value to use if the field isn't supplied.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The builder.</returns>
        public FieldDeserializationInfoBuilder<TObject, TReturn> WithDefault(object defaultValue)
        {
            _defaultValue = defaultValue;
            return this;
        }

        /// <summary>
        /// Specifies a custom method to use to map the field.
        /// </summary>
        /// <param name="customMapperFunc">The custom method to use.</param>
        /// <returns>The builder.</returns>
        public FieldDeserializationInfoBuilder<TObject, TReturn> MapUsing(Func<string, KeyValuePair<YamlNode, YamlNode>, IErrorReporter, object> customMapperFunc)
        {
            _customMapperFunc = customMapperFunc;

            return this;
        }

        // ReSharper disable once InvalidXmlDocComment
        /// <summary>
        /// Specifies an <see cref="IDeserializer"/> to use to map the field.
        /// </summary>
        /// <param name="deserializer">The deserializer.</param>
        /// <returns>The builder.</returns>
        public FieldDeserializationInfoBuilder<TObject, TReturn> MapUsingDeserializer(IDeserializer<object> deserializer)
        {
            _deserializer = deserializer;

            return this;
        }

        /// <summary>
        /// Builds the field.
        /// </summary>
        /// <returns>The field.</returns>
        public FieldDeserializationInfo Build()
        {
            return new FieldDeserializationInfo(
                _propertyInfo, _isRequired, _defaultValue, _customMapperFunc, _deserializer, _validators);
        }

        /// <summary>
        /// Indicates that this field should be validated as a Cron expression.
        /// </summary>
        public void ValidateCronExpression()
        {
            _validators.Add(new CronExpressionValidator());
        }
    }
}