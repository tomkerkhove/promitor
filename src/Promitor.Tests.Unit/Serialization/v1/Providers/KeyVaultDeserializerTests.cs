using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using System.ComponentModel;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class KeyVaultDeserializerTests : ResourceDeserializerTest<KeyVaultDeserializer>
    {
        private readonly KeyVaultDeserializer _deserializer;

        public KeyVaultDeserializerTests()
        {
            _deserializer = new KeyVaultDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_KeyVaultNameSupplied_SetsKeyVaultName()
        {
            const string keyVaultName = "promitor-key-vault";
            YamlAssert.PropertySet<KeyVaultResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"keyVaultName: {keyVaultName}",
                keyVaultName,
                r => r.KeyVaultName);
        }

        [Fact]
        public void Deserialize_KeyVaultNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<KeyVaultResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.KeyVaultName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new KeyVaultDeserializer(Logger);
        }
    }
}