using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ExpressRouteCircuitDeserializer : ResourceDeserializer<ExpressRouteCircuitResourceV1>
    {
        public ExpressRouteCircuitDeserializer(ILogger<ExpressRouteCircuitDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ExpressRouteCircuitName)
                .IsRequired();
        }
    }
}
