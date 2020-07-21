using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    public static class YamlMappingNodeExtensions
    {
        /// <summary>
        /// Gets the string value of the specified yaml property.
        /// </summary>
        /// <param name="node">The node containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The string value of the property.</returns>
        public static string GetString(this YamlMappingNode node, string propertyName)
        {
            if (node.Children.TryGetValue(propertyName, out var propertyNode))
            {
                return propertyNode.ToString();
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the specified yaml property converted to an enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of enum to return.</typeparam>
        /// <param name="node">The node containing the property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The enum value, or null if the property doesn't exist.</returns>
        public static TEnum? GetEnum<TEnum>(this YamlMappingNode node, string propertyName)
            where TEnum: struct
        {
            if (node.Children.TryGetValue(propertyName, out var propertyNode))
            {
                return Enum.Parse<TEnum>(propertyNode.ToString());
            }

            return null;
        }

        /// <summary>
        /// Gets the contents of the node.
        /// </summary>
        /// <param name="node">The node containing the property.</param>
        /// <returns>The child items of the property as a dictionary.</returns>
        public static Dictionary<string, string> GetDictionary(this YamlMappingNode node)
        {
            var result = new Dictionary<string, string>();

            foreach (var (key, value) in node.Children)
            {
                result[key.ToString()] = value.ToString();
            }

            return result;
        }

        /// <summary>
        /// Gets the contents of the specified property as a dictionary.
        /// </summary>
        /// <param name="node">The node containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The child items of the property as a dictionary.</returns>
        public static Dictionary<string, string> GetDictionary(this YamlMappingNode node, string propertyName)
        {
            if (node.Children.TryGetValue(propertyName, out var propertyNode))
            {
                return GetDictionary(((YamlMappingNode)propertyNode));
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the specified yaml property converted to a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="node">The node containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value converted to a timespan, or null if the property doesn't exist.</returns>
        public static TimeSpan? GetTimeSpan(this YamlMappingNode node, string propertyName)
        {
            if (node.Children.TryGetValue(propertyName, out var propertyNode))
            {
                return TimeSpan.Parse(propertyNode.ToString());
            }

            return null;
        }

        /// <summary>
        /// Deserializes a child object using the specified deserializer.
        /// </summary>
        /// <typeparam name="TObject">The type of object to deserialize.</typeparam>
        /// <param name="node">The yaml node.</param>
        /// <param name="propertyName">The name of the property to deserialize.</param>
        /// <param name="deserializer">The deserializer to use.</param>
        /// <param name="errorReporter">Used to report information about the deserialization process.</param>
        /// <returns>The deserialized property, or null if the property does not exist.</returns>
        public static TObject DeserializeChild<TObject>(
            this YamlMappingNode node, string propertyName, IDeserializer<TObject> deserializer, IErrorReporter errorReporter)
            where TObject: class, new()
        {
            if (node.Children.TryGetValue(propertyName, out var propertyNode))
            {
                return deserializer.Deserialize((YamlMappingNode)propertyNode, errorReporter);
            }

            return null;
        }
    }
}
