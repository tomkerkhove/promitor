using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ContainerInstanceDeserializer : ResourceDeserializer<ContainerInstanceResourceV1>
    {
        public ContainerInstanceDeserializer(ILogger<ContainerInstanceDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ContainerGroup)
                .IsRequired();
        }
    }
}
