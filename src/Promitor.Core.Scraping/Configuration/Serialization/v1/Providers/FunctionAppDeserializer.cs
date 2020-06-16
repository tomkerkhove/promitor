using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class FunctionAppDeserializer : ResourceDeserializer<FunctionAppResourceV1>
    {
        public FunctionAppDeserializer(ILogger<FunctionAppDeserializer> logger) : base(logger)
        {
            Map(resource => resource.FunctionAppName)
                .IsRequired();
            Map(resource => resource.SlotName);
        }
    }
}
