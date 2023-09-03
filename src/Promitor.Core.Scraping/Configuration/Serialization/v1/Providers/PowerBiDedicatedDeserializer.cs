using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class PowerBiDedicatedDeserializer : ResourceDeserializer<PowerBiDedicatedResourceV1>
    {
        public PowerBiDedicatedDeserializer(ILogger<PowerBiDedicatedDeserializer> logger) : base(logger)
	{
            Map(resource => resource.CapacityName)
	        .IsRequired();
	}
    }
}
