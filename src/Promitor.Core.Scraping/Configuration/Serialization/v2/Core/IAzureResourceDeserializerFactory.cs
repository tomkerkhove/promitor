using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    /// <summary>
    /// A factory for creating deserializers that can create <see cref="AzureResourceDefinitionV2"/>
    /// objects.
    /// </summary>
    public interface IAzureResourceDeserializerFactory
    {
        /// <summary>
        /// Gets a deserializer that can deserializer the specified type of resource.
        /// </summary>
        /// <param name="resourceType">The type of resource.</param>
        /// <returns>The deserializer.</returns>
        IDeserializer<AzureResourceDefinitionV2> GetDeserializerFor(ResourceType resourceType);
    }
}
