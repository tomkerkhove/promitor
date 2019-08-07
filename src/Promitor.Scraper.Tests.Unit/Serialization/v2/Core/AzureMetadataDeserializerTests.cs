using System;
using System.ComponentModel;
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
            _deserializer = new AzureMetadataDeserializer();
        }

        [Fact]
        public void Deserialize_NodeNull_ThrowsException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => _deserializer.Deserialize(null));
        }

        [Fact]
        public void Deserialize_NodeWrongType_ThrowsException()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("version: v1").Children["version"];

            // Act / Assert
            Assert.Throws<ArgumentException>(() => _deserializer.Deserialize(node));
        }

        [Fact]
        public void Deserialize_Metadata_SetsFromYaml()
        {
            // Arrange
            const string tenantId = "c8819874-9e56-4e3f-b1a8-1c0325138f27";
            const string subscriptionId = "0f9d7fea-99e8-4768-8672-06a28514f77e";
            const string resourceGroupName = "promitor";

            var yamlText =
$@"azureMetadata:
    tenantId: '{tenantId}'
    subscriptionId: '{subscriptionId}'
    resourceGroupName: '{resourceGroupName}'";
            var node = YamlUtils.CreateYamlNode(yamlText).Children["azureMetadata"];

            // Act
            var metadata = _deserializer.Deserialize(node);

            // Assert
            Assert.Equal(tenantId, metadata.TenantId);
            Assert.Equal(subscriptionId, metadata.SubscriptionId);
            Assert.Equal(resourceGroupName, metadata.ResourceGroupName);
        }
    }
}
