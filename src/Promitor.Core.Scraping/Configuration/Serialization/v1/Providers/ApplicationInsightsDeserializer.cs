using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class ApplicationInsightsDeserializer : ResourceDeserializer<ApplicationInsightsResourceV1>
    {
        public ApplicationInsightsDeserializer(ILogger<ApplicationInsightsDeserializer> logger) : base(logger)
        {
            Map(resource => resource.Name)
                .IsRequired();
        }
    }
}
