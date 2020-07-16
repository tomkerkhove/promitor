using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class LogicAppDeserializer : ResourceDeserializer<LogicAppResourceV1>
    {
        public LogicAppDeserializer(ILogger<LogicAppDeserializer> logger) : base(logger)
        {
            Map(resource => resource.WorkflowName)
                .IsRequired();
        }
    }
}
