using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization
{
    /// <summary>
    /// An object that can deserialize a yaml node into an object.
    /// </summary>
    /// <typeparam name="TObject">The type of object that can be deserialized.</typeparam>
    public interface IDeserializer<out TObject>
    {
        /// <summary>
        /// Deserializes the specified node.
        /// </summary>
        /// <param name="node">The node to deserialize.</param>
        /// <param name="errorReporter">Used to report deserialization errors.</param>
        /// <returns>The deserialized object.</returns>
        TObject Deserialize(YamlMappingNode node, IErrorReporter errorReporter);

        /// <summary>
        /// Deserializes an array of elements.
        /// </summary>
        /// <param name="node">The node to deserialize.</param>
        /// <param name="errorReporter">Used to report deserialization errors.</param>
        /// <returns>The deserialized objects.</returns>
        IReadOnlyCollection<TObject> Deserialize(YamlSequenceNode node, IErrorReporter errorReporter);
    }
}
