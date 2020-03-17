using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class GenericResourceDeserializer : ResourceDeserializer<GenericResourceV1>
    {
        public GenericResourceDeserializer(ILogger<GenericResourceDeserializer> logger) : base(logger)
        {
            MapRequired(resource => resource.ResourceUri);
            MapOptional(resource => resource.Filter);
        }
    }
}
