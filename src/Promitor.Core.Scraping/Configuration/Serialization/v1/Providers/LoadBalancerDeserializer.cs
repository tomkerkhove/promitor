using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class LoadBalancerDeserializer : ResourceDeserializer<LoadBalancerResourceV1>
    {
        public LoadBalancerDeserializer(ILogger<LoadBalancerDeserializer> logger) : base(logger)
        {
            Map(resource => resource.LoadBalancerName)
                .IsRequired();
        }
    }
}
