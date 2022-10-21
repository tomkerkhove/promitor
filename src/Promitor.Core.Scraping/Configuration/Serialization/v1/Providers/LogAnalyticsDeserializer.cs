using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class LogAnalyticsDeserializer : ResourceDeserializer<LogAnalyticsResourceV1>
    {
        public LogAnalyticsDeserializer(ILogger<LogAnalyticsDeserializer> logger) : base(logger)
        {
            Map(resource => resource.WorkspaceId)
                .IsRequired();
            Map(resource => resource.WorkspaceName)
                .IsRequired();
        }
    }
}