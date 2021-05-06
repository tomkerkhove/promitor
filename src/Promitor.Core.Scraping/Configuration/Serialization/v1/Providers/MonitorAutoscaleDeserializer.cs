using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class MonitorAutoscaleDeserializer : ResourceDeserializer<MonitorAutoscaleResourceV1>
    {
        public MonitorAutoscaleDeserializer(ILogger<MonitorAutoscaleDeserializer> logger) : base(logger)
        {
            Map(resource => resource.AutoscaleSettingsName)
                .IsRequired();
        }
    }
}
