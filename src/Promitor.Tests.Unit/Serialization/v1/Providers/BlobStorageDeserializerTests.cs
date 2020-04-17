using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    public class BlobStorageDeserializerTests : ResourceDeserializerTest<BlobStorageDeserializer>
    {
        private readonly BlobStorageDeserializer _deserializer;

        public BlobStorageDeserializerTests()
        {
            _deserializer = new BlobStorageDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_AccountNameSupplied_SetsAccountName()
        {
            const string storageAccountName = "promitor-account";
            YamlAssert.PropertySet<BlobStorageResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"accountName: {storageAccountName}",
                storageAccountName,
                r => r.AccountName);
        }

        [Fact]
        public void Deserialize_AccountNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<BlobStorageResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.AccountName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new BlobStorageDeserializer(Logger);
        }
    }
}