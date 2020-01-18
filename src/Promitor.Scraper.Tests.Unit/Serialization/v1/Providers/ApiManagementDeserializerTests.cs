using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    public class ApiManagementDeserializerTests : ResourceDeserializerTest<ApiManagementDeserializer>
    {
        private readonly ApiManagementDeserializer _deserializer;

        public ApiManagementDeserializerTests()
        {
            _deserializer = new ApiManagementDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_InstanceNameSupplied_SetsAccountName()
        {
            const string instanceName = "promitor-gateway";
            YamlAssert.PropertySet<ApiManagementResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                $"instanceName: {instanceName}",
                instanceName,
                r => r.InstanceName);
        }

        [Fact]
        public void Deserialize_InstanceNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<ApiManagementResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.InstanceName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new ApiManagementDeserializer(Logger);
        }
    }
}