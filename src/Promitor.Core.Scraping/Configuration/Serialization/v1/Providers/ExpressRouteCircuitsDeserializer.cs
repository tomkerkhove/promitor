using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ExpressRouteCircuitsDeserializer : ResourceDeserializer<ExpressRouteCircuitsResourceV1>
    {
        public ExpressRouteCircuitsDeserializer(ILogger<ExpressRouteCircuitsDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ExpressRouteCircuitsName)
                .IsRequired();
        }
    }
}
