using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class KeyVaultDeserializer : ResourceDeserializer<KeyVaultResourceV1>
    {
        public KeyVaultDeserializer(ILogger<KeyVaultDeserializer> logger) : base(logger)
        {
            MapRequired(resource => resource.VaultName);
        }
    }
}
