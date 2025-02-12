using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Serialization.Enum;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class AzureMetadataDeserializerTests : UnitTest
    {
        private readonly AzureMetadataDeserializer _deserializer;
        private readonly Mock<IErrorReporter> _errorReporter = new();
        private readonly Mock<IDeserializer<AzureEndpointsV1>> _azureEndpointsDeserializer;

        public AzureMetadataDeserializerTests()
        {
            _azureEndpointsDeserializer = new Mock<IDeserializer<AzureEndpointsV1>>();
            _deserializer = new AzureMetadataDeserializer(_azureEndpointsDeserializer.Object, NullLogger<AzureMetadataDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_AzureCloudSuppliedWithUnspecified_SetsAzureChinaCloud()
        {
            var yamlNode = YamlUtils.CreateYamlNode(
                $@"azureMetadata:
    cloud: '{AzureCloud.Unspecified}'");
            var azureMetadataNode = (YamlMappingNode)yamlNode.Children["azureMetadata"];
            var errorNode = azureMetadataNode.Children["cloud"];

            YamlAssert.ReportsError(
                _deserializer,
                azureMetadataNode,
                errorNode,
                "'Unspecified' is not a supported value for 'cloud'.");
        }

        [Fact]
        public void Deserialize_AzureCloudSupplied_SetsAzureChinaCloud()
        {
            AzureCloud azureCloud = AzureCloud.China;

            var yamlText =
                $@"azureMetadata:
    cloud: '{AzureCloud.China}'";

            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "azureMetadata",
                azureCloud,
                a => a.Cloud);
        }

        [Fact]
        public void Deserialize_AzureCloudNotSupplied_SetsGlobalAzureCloud()
        {
            AzureCloud azureCloud = AzureCloud.Global;

            var yamlText =
                @"azureMetadata:
    tenantId: ABC";

            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "azureMetadata",
                azureCloud,
                a => a.Cloud);
        }

        [Fact]
        public void Deserialize_InvalidAzureCloudSupplied_ReportsError()
        {
            var yamlNode = YamlUtils.CreateYamlNode(
                @"azureMetadata:
    cloud: invalid");
            var azureMetadataNode = (YamlMappingNode)yamlNode.Children["azureMetadata"];
            var errorNode = azureMetadataNode.Children["cloud"];

            YamlAssert.ReportsError(
                _deserializer,
                azureMetadataNode,
                errorNode,
                "'invalid' is not a valid value for 'cloud'.");
        }

        [Fact]
        public void Deserialize_TenantIdSupplied_SetsTenantId()
        {
            const string tenantId = "e0372f7f-a362-47fb-9631-74a5c4ba8bbf";

            var yamlText =
                $@"azureMetadata:
    tenantId: '{tenantId}'";

            YamlAssert.PropertySet(
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
    subscriptionId: '63c590b6-4947-4898-92a3-cae91a31b5e4'";

            YamlAssert.PropertyNull(
                _deserializer,
                yamlText,
                "azureMetadata",
                a => a.TenantId);
        }

        [Fact]
        public void Deserialize_TenantIdNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(
@"azureMetadata:
    subscriptionId: '63c590b6-4947-4898-92a3-cae91a31b5e4'");
            var metaDataNode = (YamlMappingNode)node.Children.Single(c => c.Key.ToString() == "azureMetadata").Value;

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                metaDataNode,
                "tenantId");
        }

        [Fact]
        public void Deserialize_SubscriptionIdSupplied_SetsSubscriptionId()
        {
            const string subscriptionId = "63c590b6-4947-4898-92a3-cae91a31b5e4";

            var yamlText =
$@"azureMetadata:
    subscriptionId: '{subscriptionId}'";

            YamlAssert.PropertySet(
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
    tenantId: 'e0372f7f-a362-47fb-9631-74a5c4ba8bbf'";

            YamlAssert.PropertyNull(
                _deserializer,
                yamlText,
                "azureMetadata",
                a => a.SubscriptionId);
        }

        [Fact]
        public void Deserialize_SubscriptionIdNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(
@"azureMetadata:
    tenantId: 'e0372f7f-a362-47fb-9631-74a5c4ba8bbf'");
            var metaDataNode = (YamlMappingNode)node.Children.Single(c => c.Key.ToString() == "azureMetadata").Value;

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                metaDataNode,
                "subscriptionId");
        }

        [Fact]
        public void Deserialize_ResourceGroupNameSupplied_SetsResourceGroupName()
        {
            const string resourceGroupName = "promitor-group";

            var yamlText =
$@"azureMetadata:
    resourceGroupName: '{resourceGroupName}'";

            YamlAssert.PropertySet(
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
    tenantId: 'e0372f7f-a362-47fb-9631-74a5c4ba8bbf'";

            YamlAssert.PropertyNull(
                _deserializer,
                yamlText,
                "azureMetadata",
                a => a.ResourceGroupName);
        }

        [Fact]
        public void Deserialize_ResourceGroupNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(
@"azureMetadata:
    tenantId: 'e0372f7f-a362-47fb-9631-74a5c4ba8bbf'");
            var metaDataNode = (YamlMappingNode)node.Children.Single(c => c.Key.ToString() == "azureMetadata").Value;

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                metaDataNode,
                "resourceGroupName");
        }

        [Fact]
        public void Deserialize_Endpoints_UsesDeserializer()
        {
            // Arrange
            var yamlNode = YamlUtils.CreateYamlNode(
                $@"azureMetadata:
    cloud: '{AzureCloud.Custom}'
    endpoints: 
        authenticationEndpoint: testAuth.com");
            var azureMetadataNode = (YamlMappingNode)yamlNode.Children["azureMetadata"];
            var endpointNode = (YamlMappingNode)azureMetadataNode.Children["endpoints"];
            var azureEndpoints = new AzureEndpointsV1();
            _azureEndpointsDeserializer.Setup(d => d.Deserialize(endpointNode,_errorReporter.Object)).Returns(azureEndpoints);

            // Act
            var definition = _deserializer.Deserialize(azureMetadataNode, _errorReporter.Object);

            // Assert
            Assert.Same(azureEndpoints, definition.Endpoints);
        } 
    }
}
