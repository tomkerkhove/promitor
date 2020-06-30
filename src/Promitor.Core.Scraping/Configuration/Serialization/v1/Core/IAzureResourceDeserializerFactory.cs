using Promitor.Core.Contracts;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    /// <summary>
    /// A factory for creating deserializers that can create <see cref="AzureResourceDefinitionV1"/>
    /// objects.
    /// </summary>
    public interface IAzureResourceDeserializerFactory
    {
        /// <summary>
        /// Gets a deserializer that can deserializer the specified type of resource.
        /// </summary>
        /// <param name="resourceType">The type of resource.</param>
        /// <returns>The deserializer.</returns>
        IDeserializer<AzureResourceDefinitionV1> GetDeserializerFor(ResourceType resourceType);
    }
}
