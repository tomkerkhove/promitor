using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class KubernetesServiceDeserializer : ResourceDeserializer<KubernetesServiceResourceV1>
    {
        public KubernetesServiceDeserializer(ILogger<KubernetesServiceDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ClusterName)
                .IsRequired();
        }
    }
}
