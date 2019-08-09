using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GuardNet;
using Microsoft.Extensions.Logging;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    public abstract class Deserializer<TObject> : IDeserializer<TObject>
    {
        protected ILogger Logger { get; }

        protected Deserializer(ILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));

            Logger = logger;
        }

        public abstract TObject Deserialize(YamlMappingNode node);

        public List<TObject> Deserialize(YamlSequenceNode nodes)
        {
            Guard.NotNull(nodes, nameof(nodes));

            var deserializedObjects = new List<TObject>();
            foreach (var item in nodes)
            {
                if (!(item is YamlMappingNode metricNode))
                {
                    throw new SerializationException($"Failed parsing metrics because we couldn't cast an item to {nameof(YamlMappingNode)}");
                }

                var deserializedObject = Deserialize(metricNode);
                deserializedObjects.Add(deserializedObject);
            }

            return deserializedObjects;
        }

        /// <summary>
        /// Gets the string value of the specified yaml property.
        /// </summary>
        /// <param name="node">The node containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The string value of the property.</returns>
        protected static string GetString(YamlMappingNode node, string propertyName)
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
        /// <typeparam name="T">The type of enum to return.</typeparam>
        /// <param name="node">The node containing the property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The enum value, or default(T) if the property doesn't exist.</returns>
        protected static T GetEnum<T>(YamlMappingNode node, string propertyName)
            where T : struct
        {
            if (node.Children.TryGetValue(propertyName, out var propertyNode))
            {
                if (System.Enum.TryParse<T>(propertyNode.ToString(), out var enumResult))
                {
                    return enumResult;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Gets the value of the specified yaml property converted to an enum.
        /// </summary>
        /// <typeparam name="T">The type of enum to return.</typeparam>
        /// <param name="node">The node containing the property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The enum value, or null if the property doesn't exist.</returns>
        protected static T? GetNullableEnum<T>(YamlMappingNode node, string propertyName)
            where T : struct
        {
            if (node.Children.TryGetValue(propertyName, out var propertyNode))
            {
                if (System.Enum.TryParse<T>(propertyNode.ToString(), out var enumResult))
                {
                    return enumResult;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the contents of the specified property as a dictionary.
        /// </summary>
        /// <param name="node">The node containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The child items of the property as a dictionary.</returns>
        protected static Dictionary<string, string> GetDictionary(YamlMappingNode node, string propertyName)
        {
            if (node.Children.TryGetValue(propertyName, out var propertyNode))
            {
                var result = new Dictionary<string, string>();

                foreach (var (key, value) in ((YamlMappingNode)propertyNode).Children)
                {
                    result[key.ToString()] = value.ToString();
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the specified yaml property converted to a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="node">The node containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value converted to a timespan, or null if the property doesn't exist.</returns>
        protected static TimeSpan? GetTimespan(YamlMappingNode node, string propertyName)
        {
            if (node.Children.TryGetValue(propertyName, out var propertyNode))
            {
                return TimeSpan.Parse(propertyNode.ToString());
            }

            return null;
        }
    }
}