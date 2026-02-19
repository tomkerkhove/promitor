using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class SearchServiceDeserializer : ResourceDeserializer<SearchServiceResourceV1>
    {
        public SearchServiceDeserializer(ILogger<SearchServiceDeserializer> logger) : base(logger)
        {
            Map(resource => resource.SearchServiceName)
                .IsRequired();
        }
    }
}
