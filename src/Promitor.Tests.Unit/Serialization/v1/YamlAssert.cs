using System;
using System.Linq;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.v1
{
    public static class YamlAssert
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
        public static void PropertySet<TObject, TResult>(
            IDeserializer<TObject> deserializer, string yamlText, TResult expected, Func<TObject, TResult> propertyAccessor)
            where TObject: new()
        {
            // Arrange
            var errorReporter = new Mock<IErrorReporter>();
            var node = YamlUtils.CreateYamlNode(yamlText);

            // Act
            var definition = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal(expected, propertyAccessor(definition));
        }

        /// <summary>
        /// Deserializes the yaml and asserts that the specified property has been set.
        /// Use this overload where the deserializer actually returns a subclass of <typeparamref name="TBaseObject"/>.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TBaseObject"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="yamlText"></param>
        /// <param name="expected"></param>
        /// <param name="propertyAccessor"></param>
        public static void PropertySet<TObject, TBaseObject, TResult>(
            IDeserializer<TBaseObject> deserializer, string yamlText, TResult expected, Func<TObject, TResult> propertyAccessor)
            where TObject: TBaseObject
            where TBaseObject: new()
        {
            // Arrange
            var errorReporter = new Mock<IErrorReporter>();
            var node = YamlUtils.CreateYamlNode(yamlText);

            // Act
            var definition = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Equal(expected, propertyAccessor((TObject)definition));
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
        public static void PropertySet<TObject, TResult>(
            IDeserializer<TObject> deserializer, string yamlText, string yamlElement, TResult expected, Func<TObject, TResult> propertyAccessor)
            where TObject: new()
        {
            // Arrange
            var errorReporter = new Mock<IErrorReporter>();
            var node = YamlUtils.CreateYamlNode(yamlText).Children[yamlElement];

            // Act
            var definition = deserializer.Deserialize((YamlMappingNode)node, errorReporter.Object);

            // Assert
            Assert.Equal(expected, propertyAccessor(definition));
        }

        /// <summary>
        /// Deserializes the yaml and verifies that the specified property is null.
        /// </summary>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        /// <param name="deserializer">The deserializer.</param>
        /// <param name="yamlText">The yaml to deserialize.</param>
        /// <param name="propertyAccessor">The property to check.</param>
        public static void PropertyNull<TObject>(
            IDeserializer<TObject> deserializer, string yamlText, Func<TObject, object> propertyAccessor)
            where TObject: new()
        {
            // Arrange
            var errorReporter = new Mock<IErrorReporter>();
            var node = YamlUtils.CreateYamlNode(yamlText);

            // Act
            var definition = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Null(propertyAccessor(definition));
        }

        /// <summary>
        /// Deserializes the yaml and verifies that the specified property is null.
        /// </summary>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        /// <typeparam name="TBaseObject">The type that the deserializer returns.</typeparam>
        /// <param name="deserializer">The deserializer.</param>
        /// <param name="yamlText">The yaml to deserialize.</param>
        /// <param name="propertyAccessor">The property to check.</param>
        public static void PropertyNull<TObject, TBaseObject>(
            IDeserializer<TBaseObject> deserializer, string yamlText, Func<TObject, object> propertyAccessor)
            where TObject: TBaseObject
            where TBaseObject: new()
        {
            // Arrange
            var errorReporter = new Mock<IErrorReporter>();
            var node = YamlUtils.CreateYamlNode(yamlText);

            // Act
            var definition = deserializer.Deserialize(node, errorReporter.Object);

            // Assert
            Assert.Null(propertyAccessor((TObject)definition));
        }

        /// <summary>
        /// Deserializes the yaml and verifies that the specified property nested
        /// under the specified yaml element is null.
        /// </summary>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        /// <param name="deserializer">The deserializer.</param>
        /// <param name="yamlText">The yaml to deserialize.</param>
        /// <param name="propertyAccessor">The property to check.</param>
        /// <param name="yamlElement">The element to look for the property under.</param>
        public static void PropertyNull<TObject>(
            IDeserializer<TObject> deserializer, string yamlText, string yamlElement, Func<TObject, object> propertyAccessor)
            where TObject: new()
        {
            // Arrange
            var errorReporter = new Mock<IErrorReporter>();
            var node = YamlUtils.CreateYamlNode(yamlText).Children[yamlElement];

            // Act
            var definition = deserializer.Deserialize((YamlMappingNode)node, errorReporter.Object);

            // Assert
            Assert.Null(propertyAccessor(definition));
        }

        /// <summary>
        /// Checks that the specified error message is reported while deserializing the yaml.
        /// </summary>
        /// <param name="deserializer">The deserializer to use.</param>
        /// <param name="yamlNode">The Yaml to deserialize.</param>
        /// <param name="errorNode">The node that should have the error.</param>
        /// <param name="expectedMessage">The message that should be reported.</param>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        public static void ReportsError<TObject>(
            IDeserializer<TObject> deserializer, YamlMappingNode yamlNode, YamlNode errorNode, string expectedMessage)
            where TObject: new()
        {
            ReportsMessage(deserializer, yamlNode, errorNode, expectedMessage, MessageType.Error);
        }

        /// <summary>
        /// Checks that the specified warning message is reported while deserializing the yaml.
        /// </summary>
        /// <param name="deserializer">The deserializer to use.</param>
        /// <param name="yamlNode">The Yaml to deserialize.</param>
        /// <param name="errorNode">The node that should have the warning.</param>
        /// <param name="expectedMessage">The message that should be reported.</param>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        public static void ReportsWarning<TObject>(
            IDeserializer<TObject> deserializer, YamlMappingNode yamlNode, YamlNode errorNode, string expectedMessage)
            where TObject: new()
        {
            ReportsMessage(deserializer, yamlNode, errorNode, expectedMessage, MessageType.Warning);
        }

        /// <summary>
        /// Checks that the specified error message is reported while deserializing the yaml.
        /// </summary>
        /// <param name="deserializer">The deserializer to use.</param>
        /// <param name="yamlNode">The Yaml to deserialize.</param>
        /// <param name="errorNode">The node that should have the error.</param>
        /// <param name="expectedMessage">The message that should be reported.</param>
        /// <param name="expectedMessageType">The type of message that should be reported.</param>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        public static void ReportsMessage<TObject>(
            IDeserializer<TObject> deserializer, YamlMappingNode yamlNode, YamlNode errorNode, string expectedMessage, MessageType expectedMessageType)
            where TObject: new()
        {
            // Arrange
            var errorReporter = new ErrorReporter();

            // Act
            deserializer.Deserialize(yamlNode, errorReporter);

            // Assert
            var message = errorReporter.Messages.FirstOrDefault(m => m.Node == errorNode && m.Message == expectedMessage);
            Assert.True(message != null, "Error message not found against specified yaml element.");
            Assert.Equal(expectedMessageType, message.MessageType);
        }

        /// <summary>
        /// Checks that an error is reported for the specified property while deserializing the yaml.
        /// </summary>
        /// <param name="deserializer">The deserializer to use.</param>
        /// <param name="yamlNode">The Yaml to deserialize.</param>
        /// <param name="propertyName">The property that should have an error.</param>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        public static void ReportsErrorForProperty<TObject>(
            IDeserializer<TObject> deserializer, YamlMappingNode yamlNode, string propertyName)
            where TObject : new()
        {
            // Arrange
            var errorReporter = new ErrorReporter();

            // Act
            deserializer.Deserialize(yamlNode, errorReporter);

            // Assert
            var message = errorReporter.Messages.FirstOrDefault(m => ReferenceEquals(m.Node, yamlNode) && m.Message.Contains(propertyName));
            Assert.True(message != null, "Error message not found against specified yaml element.");
            Assert.Equal(MessageType.Error, message.MessageType);
        }

        /// <summary>
        /// Checks that an error is reported for the specified property while deserializing the yaml.
        /// </summary>
        /// <param name="deserializer">The deserializer to use.</param>
        /// <param name="yamlNode">The Yaml to deserialize.</param>
        /// <param name="propertyName">The property that should have an error.</param>
        /// <typeparam name="TObject">The type of object being deserialized.</typeparam>
        public static void ReportsNoErrorForProperty<TObject>(
            IDeserializer<TObject> deserializer, YamlMappingNode yamlNode, string propertyName)
            where TObject : new()
        {
            // Arrange
            var errorReporter = new ErrorReporter();

            // Act
            deserializer.Deserialize(yamlNode, errorReporter);

            // Assert
            var message = errorReporter.Messages.FirstOrDefault(m => ReferenceEquals(m.Node, yamlNode) && m.Message.Contains(propertyName));
            Assert.False(message != null, "Error message not found against specified yaml element.");
        }
    }
}
