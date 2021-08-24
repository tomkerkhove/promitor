using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class DataFactoryDeserializer : ResourceDeserializer<DataFactoryResourceV1>
    {
        public DataFactoryDeserializer(ILogger<DataFactoryDeserializer> logger) : base(logger)
        {
            Map(resource => resource.FactoryName)
                .IsRequired();
        }
    }
}
