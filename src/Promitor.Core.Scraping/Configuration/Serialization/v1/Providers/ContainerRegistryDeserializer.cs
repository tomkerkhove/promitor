using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ContainerRegistryDeserializer : ResourceDeserializer<ContainerRegistryResourceV1>
    {
        public ContainerRegistryDeserializer(ILogger<ContainerRegistryDeserializer> logger) : base(logger)
        {
            Map(resource => resource.RegistryName)
                .IsRequired();
        }
    }
}
