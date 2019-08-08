using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    /// <summary>
    /// An object that can deserialize a yaml node into an object.
    /// </summary>
    /// <typeparam name="T">The type of object that can be deserialized.</typeparam>
    public interface IDeserializer<T>
    {
        /// <summary>
        /// Deserializes the specified node.
        /// </summary>
        /// <param name="node">The node to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        T Deserialize(YamlMappingNode node);

        /// <summary>
        /// Deserializes an array of elements.
        /// </summary>
        /// <param name="node">The node to deserialize.</param>
        /// <returns>The deserialized objects.</returns>
        List<T> Deserialize(YamlSequenceNode node);
    }
}
