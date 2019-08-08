using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    /// <summary>
    /// A base class for azure resource deserializers that makes sure that any shared
    /// properties are deserialized correctly for all resources.
    /// </summary>
    public abstract class ResourceDeserializer : Deserializer<AzureResourceDefinitionV2>
    {
        private const string ResourceGroupNameTag = "resourceGroupName";

        protected ResourceDeserializer(ILogger logger) : base(logger)
        {
        }

        public override AzureResourceDefinitionV2 Deserialize(YamlMappingNode node)
        {
            var resource = DeserializeResource(node);

            resource.ResourceGroupName = GetString(node, ResourceGroupNameTag);

            return resource;
        }

        /// <summary>
        /// Implement on subclasses to return the correct type of <see cref="AzureResourceDefinitionV2"/>
        /// object with all its custom properties populated.
        /// </summary>
        /// <param name="node">The yaml node.</param>
        /// <returns>The deserialized object.</returns>
        protected abstract AzureResourceDefinitionV2 DeserializeResource(YamlMappingNode node);
    }
}
