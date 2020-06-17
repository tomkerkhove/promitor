using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class AzureResourceCollectionDeserializer : Deserializer<AzureResourceCollectionDefinitionV1>
    {
        public AzureResourceCollectionDeserializer(ILogger<AzureResourceCollectionDeserializer> logger) : base(logger)
        {
            Map(metadata => metadata.Name)
                .IsRequired();
        }
    }
}
