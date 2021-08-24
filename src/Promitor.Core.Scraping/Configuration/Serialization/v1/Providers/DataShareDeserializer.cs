using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class DataShareDeserializer : ResourceDeserializer<DataShareResourceV1>
    {
        public DataShareDeserializer(ILogger<DataShareDeserializer> logger) : base(logger)
        {
            Map(resource => resource.ShareName)
                .IsRequired();
        }
    }
}
