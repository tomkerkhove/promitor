using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class EventHubClusterDeserializer : ResourceDeserializer<EventHubClusterResourceV1>
    {
        public EventHubClusterDeserializer(ILogger<EventHubClusterDeserializer> logger) : base(logger)
        {
        }

        protected void DeserializeAdditionalProperties(EventHubClusterResourceV1 resource, IDictionary<string, object> additionalProperties)
        {
            if (additionalProperties.TryGetValue("clusterName", out var clusterName))
            {
                resource.ClusterName = clusterName.ToString();
            }
        }
    }
}