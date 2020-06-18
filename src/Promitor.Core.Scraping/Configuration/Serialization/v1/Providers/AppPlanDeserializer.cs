using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class AppPlanDeserializer : ResourceDeserializer<AppPlanResourceV1>
    {
        public AppPlanDeserializer(ILogger<AppPlanDeserializer> logger) : base(logger)
        {
            Map(resource => resource.AppPlanName)
                .IsRequired();
        }
    }
}
