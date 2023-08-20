using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
	public class PowerBiEmbeddedDeserializer : ResourceDeserializer<PowerBiEmbeddedResourceV1>
	{
		public PowerBiEmbeddedDeserializer(ILogger<PowerBiEmbeddedDeserializer> logger) : base(logger)
		{
            Map(resource => resource.CapacityName)
				.IsRequired();
		}
	}
}