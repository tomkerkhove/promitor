﻿using System.ComponentModel;
using System.Linq;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Serialization.Enum;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class AzureMetadataDeserializerTests : UnitTest
    {
        private readonly AzureMetadataDeserializer _deserializer;

        public AzureMetadataDeserializerTests()
        {
            _deserializer = new AzureMetadataDeserializer(NullLogger<AzureMetadataDeserializer>.Instance);
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
            AzureEnvironment azureCloud = AzureEnvironment.AzureChinaCloud;

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
            AzureEnvironment azureCloud = AzureEnvironment.AzureGlobalCloud;

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
            const string tenantId = "c8819874-9e56-4e3f-b1a8-1c0325138f27";

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
    subscriptionId: '0f9d7fea-99e8-4768-8672-06a28514f77e'";

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
    subscriptionId: '0f9d7fea-99e8-4768-8672-06a28514f77e'");
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
            const string subscriptionId = "0f9d7fea-99e8-4768-8672-06a28514f77e";

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
    tenantId: 'c8819874-9e56-4e3f-b1a8-1c0325138f27'";

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
    tenantId: 'c8819874-9e56-4e3f-b1a8-1c0325138f27'");
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
    tenantId: 'c8819874-9e56-4e3f-b1a8-1c0325138f27'";

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
    tenantId: 'c8819874-9e56-4e3f-b1a8-1c0325138f27'");
            var metaDataNode = (YamlMappingNode)node.Children.Single(c => c.Key.ToString() == "azureMetadata").Value;

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                metaDataNode,
                "resourceGroupName");
        }
    }
}
