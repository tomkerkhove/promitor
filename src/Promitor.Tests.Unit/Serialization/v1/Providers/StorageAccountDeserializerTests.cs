using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    public class StorageAccountDeserializerTests : ResourceDeserializerTest<StorageAccountDeserializer>
    {
        private readonly StorageAccountDeserializer _deserializer;

        public StorageAccountDeserializerTests()
        {
            _deserializer = new StorageAccountDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_AccountNameSupplied_SetsAccountName()
        {
            const string storageAccountName = "promitor-account";
            YamlAssert.PropertySet<StorageAccountResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"accountName: {storageAccountName}",
                storageAccountName,
                r => r.AccountName);
        }

        [Fact]
        public void Deserialize_AccountNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<StorageAccountResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.AccountName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new StorageAccountDeserializer(Logger);
        }
    }
}