using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class CognitiveServicesAccountDeserializer : ResourceDeserializer<CognitiveServicesAccountResourceV1>
    {
        public CognitiveServicesAccountDeserializer(ILogger<CognitiveServicesAccountDeserializer> logger) : base(logger)
        {
            Map(resource => resource.CognitiveServicesAccountName)
                .IsRequired();
        }
    }
}
