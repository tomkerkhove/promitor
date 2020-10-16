using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class KubernetesDeserializer : ResourceDeserializer<KubernetesResourceV1>
    {
        public KubernetesDeserializer(ILogger<KubernetesDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ClusterName)
                .IsRequired();
        }
    }
}
