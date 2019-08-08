using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Core;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Core
{
    [Category("Unit")]
    public class AzureMetadataDeserializerTests
    {
        private readonly AzureMetadataDeserializer _deserializer;

        public AzureMetadataDeserializerTests()
        {
            _deserializer = new AzureMetadataDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_TenantIdSupplied_SetsTenantId()
        {
            const string tenantId = "c8819874-9e56-4e3f-b1a8-1c0325138f27";

            var yamlText =
$@"azureMetadata:
    tenantId: '{tenantId}'";

            DeserializerTestHelpers.AssertPropertySet(
                _deserializer,
                yamlText,
                "azureMetadata",
                tenantId,
                a => a.TenantId);
        }

        [Fact]
        public void Deserialize_TenantIdNotSupplied_Null()
        {
            const string yamlText =
@"azureMetadata:
    subscriptionId: '0f9d7fea-99e8-4768-8672-06a28514f77e'";

            DeserializerTestHelpers.AssertPropertyNull(
                _deserializer,
                yamlText,
                "azureMetadata",
                a => a.TenantId);
        }

        [Fact]
        public void Deserialize_SubscriptionIdSupplied_SetsSubscriptionId()
        {
            const string subscriptionId = "0f9d7fea-99e8-4768-8672-06a28514f77e";

            var yamlText =
$@"azureMetadata:
    subscriptionId: '{subscriptionId}'";

            DeserializerTestHelpers.AssertPropertySet(
                _deserializer,
                yamlText,
                "azureMetadata",
                subscriptionId,
                a => a.SubscriptionId);
        }

        [Fact]
        public void Deserialize_SubscriptionIdNotSupplied_Null()
        {
            const string yamlText =
@"azureMetadata:
    tenantId: 'c8819874-9e56-4e3f-b1a8-1c0325138f27'";

            DeserializerTestHelpers.AssertPropertyNull(
                _deserializer,
                yamlText,
                "azureMetadata",
                a => a.SubscriptionId);
        }

        [Fact]
        public void Deserialize_ResourceGroupNameSupplied_SetsResourceGroupName()
        {
            const string resourceGroupName = "promitor-group";

            var yamlText =
$@"azureMetadata:
    resourceGroupName: '{resourceGroupName}'";

            DeserializerTestHelpers.AssertPropertySet(
                _deserializer,
                yamlText,
                "azureMetadata",
                resourceGroupName,
                a => a.ResourceGroupName);
        }

        [Fact]
        public void Deserialize_ResourceGroupNameNotSupplied_Null()
        {
            const string yamlText =
@"azureMetadata:
    tenantId: 'c8819874-9e56-4e3f-b1a8-1c0325138f27'";

            DeserializerTestHelpers.AssertPropertyNull(
                _deserializer,
                yamlText,
                "azureMetadata",
                a => a.ResourceGroupName);
        }
    }
}
