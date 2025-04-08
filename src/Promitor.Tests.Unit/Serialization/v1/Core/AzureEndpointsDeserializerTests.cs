using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class AzureEndpointsDeserializerTests : UnitTest
    {
        private readonly AzureEndpointsDeserializer _deserializer;
        public AzureEndpointsDeserializerTests() 
        {
            _deserializer = new AzureEndpointsDeserializer(NullLogger<AzureEndpointsDeserializer>.Instance);
        }


        [Fact]
        public void Deserialize_NotSuppliedWithRequiredProperty_Fails()
        {
            // Arrange
            var yamlNode = YamlUtils.CreateYamlNode(
                $@"endpoints:
    authenticationEndpoint: https://ab.core.windows.net");
            var endpointsNode = (YamlMappingNode)yamlNode.Children.Single(c => c.Key.ToString() == "endpoints").Value;

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                endpointsNode,
                "managementEndpoint");
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                endpointsNode,
                "resourceManagerEndpoint");
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                endpointsNode,
                "resourceManagerEndpoint");
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                endpointsNode,
                "graphEndpoint");
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                endpointsNode,
                "storageEndpointSuffix");
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                endpointsNode,
                "keyVaultSuffix");
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                endpointsNode,
                "metricsQueryAudience");
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                endpointsNode,
                "metricsClientAudience");
        }

        [Fact]
        public void Deserialize_NotSuppliedWithOptionalProperty_Succeeds()
        {
            // Arrange
            var yamlNode = YamlUtils.CreateYamlNode(
                $@"endpoints:
    authenticationEndpoint: https://ab.core.windows.net");
            var endpointsNode = (YamlMappingNode)yamlNode.Children.Single(c => c.Key.ToString() == "endpoints").Value;

            // Act / Assert
            YamlAssert.ReportsNoErrorForProperty(
                _deserializer,
                endpointsNode,
                "logAnalyticsEndpoint");
        }

        [Fact]
        public void Deserialize_SuppliedWithInvalidUrl_Fails()
        {
            // Arrange
            var authEndpoint = "test_auth";
            var managementEndpoint = "test_mgmt.com";
            var resourceManagerEndpoint = "test_resource";
            var graphEndpoint = "test_graph";
            var metricsQueryAudience = "test_metrics_query";
            var metricsClientAudience = "test_metrics_client";
            var logAnalyticsEndpoint = "test_log_analytics";

            var yamlNode = YamlUtils.CreateYamlNode(
                $@"endpoints:
    authenticationEndpoint: '{authEndpoint}'
    managementEndpoint: '{managementEndpoint}'
    resourceManagerEndpoint: '{resourceManagerEndpoint}'
    graphEndpoint: '{graphEndpoint}'
    metricsQueryAudience: '{metricsQueryAudience}'
    metricsClientAudience: '{metricsClientAudience}'
    logAnalyticsEndpoint: '{logAnalyticsEndpoint}'");

            var endpointsNode = (YamlMappingNode)yamlNode.Children["endpoints"];

            // Act / Assert
            YamlAssert.ReportsError(
                _deserializer,
                endpointsNode,
                endpointsNode.Children["authenticationEndpoint"],
                $"'{authEndpoint}' is not a valid URL for authenticationEndpoint.");
            YamlAssert.ReportsError(
                _deserializer,
                endpointsNode,
                endpointsNode.Children["managementEndpoint"],
                $"'{managementEndpoint}' is not a valid URL for managementEndpoint.");
            YamlAssert.ReportsError(
                _deserializer,
                endpointsNode,
                endpointsNode.Children["resourceManagerEndpoint"],
                $"'{resourceManagerEndpoint}' is not a valid URL for resourceManagerEndpoint.");
            YamlAssert.ReportsError(
                _deserializer,
                endpointsNode,
                endpointsNode.Children["graphEndpoint"],
                $"'{graphEndpoint}' is not a valid URL for graphEndpoint.");
            YamlAssert.ReportsError(
                _deserializer,
                endpointsNode,
                endpointsNode.Children["metricsQueryAudience"],
                $"'{metricsQueryAudience}' is not a valid URL for metricsQueryAudience.");
            YamlAssert.ReportsError(
                _deserializer,
                endpointsNode,
                endpointsNode.Children["metricsClientAudience"],
                $"'{metricsClientAudience}' is not a valid URL for metricsClientAudience.");
            YamlAssert.ReportsError(
                _deserializer,
                endpointsNode,
                endpointsNode.Children["logAnalyticsEndpoint"],
                $"'{logAnalyticsEndpoint}' is not a valid URL for logAnalyticsEndpoint.");
        }

        [Fact]
        public void Deserialize_SuppliedWithValidValues_SetsProperty() 
        {
            // Arrange
            var authEndpoint = "https://auth.endpoint.com/";
            var managementEndpoint = "https://management.endpoint.net/";
            var resourceManagerEndpoint = "https://management.endpoint.com/";
            var graphEndpoint = "https://graph.endpoint.com/";
            var metricsQueryAudience = "https://metric.query.endpoint.com/";
            var metricsClientAudience = "https://metric.client.endpoint.com/";
            var storageEndpointSuffix = "core.windows.net";
            var keyVaultSuffix = "vault.azure.net";
            var logAnalyticsEndpoint = "https://loganalytics.endpoint.com/";

            var yamlText =
                $@"endpoints:
    authenticationEndpoint: '{authEndpoint}'
    managementEndpoint: '{managementEndpoint}'
    resourceManagerEndpoint: '{resourceManagerEndpoint}'
    graphEndpoint: '{graphEndpoint}'
    metricsQueryAudience: '{metricsQueryAudience}'
    metricsClientAudience: '{metricsClientAudience}'
    storageEndpointSuffix: '{storageEndpointSuffix}'
    keyVaultSuffix: '{keyVaultSuffix}'
    logAnalyticsEndpoint: '{logAnalyticsEndpoint}'";

            // Act / Assert
            YamlAssert.PropertySet(
               _deserializer,
               yamlText,
               "endpoints",
               authEndpoint,
               a => a.AuthenticationEndpoint
               );
            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "endpoints",
                managementEndpoint,
                a => a.ManagementEndpoint
                );
            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "endpoints",
                resourceManagerEndpoint,
                a => a.ResourceManagerEndpoint
                );
            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "endpoints",
                graphEndpoint,
                a => a.GraphEndpoint
                );
            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "endpoints",
                metricsQueryAudience,
                a => a.MetricsQueryAudience
                );
            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "endpoints",
                metricsClientAudience,
                a => a.MetricsClientAudience
                );
            YamlAssert.PropertySet(
               _deserializer,
               yamlText,
               "endpoints",
               storageEndpointSuffix,
               a => a.StorageEndpointSuffix
               );
            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "endpoints",
                keyVaultSuffix,
                a => a.KeyVaultSuffix
                );
            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "endpoints",
                logAnalyticsEndpoint,
                a => a.LogAnalyticsEndpoint
                );
        }
    }
}
