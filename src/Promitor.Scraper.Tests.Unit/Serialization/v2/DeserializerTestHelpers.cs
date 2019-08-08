using System;
using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2
{
    public static class DeserializerTestHelpers
    {
        /// <summary>
        /// Deserializes the yaml using the deserializer, and asserts that the
        /// specified property has been set.
        /// </summary>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        /// <typeparam name="TResult">The property type.</typeparam>
        /// <param name="deserializer">The deserializer.</param>
        /// <param name="yamlText">The yaml to deserialize.</param>
        /// <param name="expected">The expected result.</param>
        /// <param name="propertyAccessor">The property to check.</param>
        public static void AssertPropertySet<TObject, TResult>(
            IDeserializer<TObject> deserializer, string yamlText, TResult expected, Func<TObject, TResult> propertyAccessor)
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(yamlText);

            // Act
            var definition = deserializer.Deserialize(node);

            // Assert
            Assert.Equal(expected, propertyAccessor(definition));
        }

        /// <summary>
        /// Deserializes the yaml using the deserializer, and asserts that the
        /// specified property underneath the specified yaml element has been set.
        /// </summary>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        /// <typeparam name="TResult">The property type.</typeparam>
        /// <param name="deserializer">The deserializer.</param>
        /// <param name="yamlText">The yaml to deserialize.</param>
        /// <param name="yamlElement">The element to find the properties under.</param>
        /// <param name="expected">The expected result.</param>
        /// <param name="propertyAccessor">The property to check.</param>
        public static void AssertPropertySet<TObject, TResult>(
            IDeserializer<TObject> deserializer, string yamlText, string yamlElement, TResult expected, Func<TObject, TResult> propertyAccessor)
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(yamlText).Children[yamlElement];

            // Act
            var definition = deserializer.Deserialize((YamlMappingNode)node);

            // Assert
            Assert.Equal(expected, propertyAccessor(definition));
        }

        /// <summary>
        /// Deserializes the yaml and verifies that the specified property is null.
        /// </summary>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        /// <typeparam name="TResult">The type of the property.</typeparam>
        /// <param name="deserializer">The deserializer.</param>
        /// <param name="yamlText">The yaml to deserialize.</param>
        /// <param name="propertyAccessor">The property to check.</param>
        public static void AssertPropertyNull<TObject, TResult>(
            IDeserializer<TObject> deserializer, string yamlText, Func<TObject, TResult> propertyAccessor)
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(yamlText);

            // Act
            var definition = deserializer.Deserialize(node);

            // Assert
            Assert.Null(propertyAccessor(definition));
        }

        /// <summary>
        /// Deserializes the yaml and verifies that the specified property nested
        /// under the specified yaml element is null.
        /// </summary>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        /// <typeparam name="TResult">The type of the property.</typeparam>
        /// <param name="deserializer">The deserializer.</param>
        /// <param name="yamlText">The yaml to deserialize.</param>
        /// <param name="propertyAccessor">The property to check.</param>
        /// <param name="yamlElement">The element to look for the property under.</param>
        public static void AssertPropertyNull<TObject, TResult>(
            IDeserializer<TObject> deserializer, string yamlText, string yamlElement, Func<TObject, TResult> propertyAccessor)
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(yamlText).Children[yamlElement];

            // Act
            var definition = deserializer.Deserialize((YamlMappingNode)node);

            // Assert
            Assert.Null(propertyAccessor(definition));
        }
    }
}
