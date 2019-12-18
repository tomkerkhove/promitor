using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ContainerRegistryDeserializer : ResourceDeserializer<ContainerRegistryResourceV1>
    {
        public ContainerRegistryDeserializer(ILogger<ContainerRegistryDeserializer> logger) : base(logger)
        {
            MapRequired(resource => resource.RegistryName);
        }
    }
}
