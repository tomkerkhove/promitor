using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Contracts;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class MetricDefinitionDeserializerTests : UnitTest
    {
        private readonly Mock<IDeserializer<AzureMetricConfigurationV1>> _azureMetricConfigurationDeserializer;
        private readonly Mock<IDeserializer<ScrapingV1>> _scrapingDeserializer;
        private readonly Mock<IAzureResourceDeserializerFactory> _resourceDeserializerFactory;
        private readonly Mock<IErrorReporter> _errorReporter = new();
        private readonly Mock<IDeserializer<LogAnalyticsConfigurationV1>> _logAnalyticsConfigurationDeserializer = new();
        private readonly Mock<IDeserializer<AzureResourceDiscoveryGroupDefinitionV1>> _resourceDiscoveryGroupDeserializer =
            new();

        private readonly MetricDefinitionDeserializer _deserializer;

        public MetricDefinitionDeserializerTests()
        {
            _azureMetricConfigurationDeserializer = new Mock<IDeserializer<AzureMetricConfigurationV1>>();
            _scrapingDeserializer = new Mock<IDeserializer<ScrapingV1>>();
            _resourceDeserializerFactory = new Mock<IAzureResourceDeserializerFactory>();

            _deserializer = new MetricDefinitionDeserializer(
                _azureMetricConfigurationDeserializer.Object,
                _logAnalyticsConfigurationDeserializer.Object,
                _scrapingDeserializer.Object,
                _resourceDiscoveryGroupDeserializer.Object,
                _resourceDeserializerFactory.Object,
                NullLogger<MetricDefinitionDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_NameSupplied_SetsName()
        {
            YamlAssert.PropertySet(
                _deserializer,
                "name: promitor_test_metric",
                "promitor_test_metric",
                d => d.Name);
        }

        [Fact]
        public void Deserialize_NameNotSupplied_Null()
        {
            YamlAssert.PropertyNull(_deserializer, "description: 'Test metric'", d => d.Name);
        }

        [Fact]
        public void Deserialize_NameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("description: 'Test metric'");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "name");
        }

        [Fact]
        public void Deserialize_DescriptionSupplied_SetsDescription()
        {
            YamlAssert.PropertySet(
                _deserializer,
                "description: 'This is a test metric'",
                "This is a test metric",
                d => d.Description);
        }

        [Fact]
        public void Deserialize_DescriptionNotSupplied_Null()
        {
            YamlAssert.PropertyNull(_deserializer, "name: metric", d => d.Description);
        }

        [Fact]
        public void Deserialize_DescriptionNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: 'test_metric'");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "description");
        }

        [Fact]
        public void Deserialize_ResourceTypeSupplied_SetsResourceType()
        {
            YamlAssert.PropertySet(
                _deserializer,
                "resourceType: ServiceBusNamespace",
                ResourceType.ServiceBusNamespace,
                d => d.ResourceType);
        }

        [Fact]
        public void Deserialize_ResourceTypeNotSupplied_Null()
        {
            YamlAssert.PropertyNull(
                _deserializer,
                "name: promitor_test_metric",
                d => d.ResourceType);
        }

        [Fact]
        public void Deserialize_ResourceTypeNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: 'test_metric'");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "resourceType");
        }

        [Fact]
        public void Deserialize_ResourceType_NotSpecified_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceType: 'NotSpecified'");

            // Act / Assert
            YamlAssert.ReportsError(
                _deserializer,
                node,
                node["resourceType"],
                "'resourceType' must not be set to 'NotSpecified'.");
        }

        [Fact]
        public void Deserialize_LabelsSupplied_SetsLabels()
        {
            const string yamlText =
@"labels:
    app: promitor
    env: test";

            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                new Dictionary<string, string> { { "app", "promitor" }, { "env", "test" } },
                d => d.Labels);
        }

        [Fact]
        public void Deserialize_LabelsNotSupplied_Null()
        {
            YamlAssert.PropertyNull(_deserializer, "name: promitor_test_metric", d => d.Labels);
        }

        [Fact]
        public void Deserialize_AzureMetricConfigurationSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
@"azureMetricConfiguration:
    metricName: ActiveMessages";
            var node = YamlUtils.CreateYamlNode(yamlText);
            var configurationNode = (YamlMappingNode)node.Children["azureMetricConfiguration"];
            var configuration = new AzureMetricConfigurationV1();

            _azureMetricConfigurationDeserializer.Setup(d => d.Deserialize(configurationNode, _errorReporter.Object)).Returns(configuration);

            // Act
            var definition = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Same(configuration, definition.AzureMetricConfiguration);
        }

        [Fact]
        public void Deserialize_AzureMetricConfigurationNotSupplied_Null()
        {
            // Arrange
            const string yamlText = @"name: promitor_test_metric";
            var node = YamlUtils.CreateYamlNode(yamlText);

            _azureMetricConfigurationDeserializer.Setup(
                d => d.Deserialize(It.IsAny<YamlMappingNode>(), It.IsAny<IErrorReporter>())).Returns(new AzureMetricConfigurationV1());

            // Act
            var definition = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Null(definition.AzureMetricConfiguration);
        }

        [Fact]
        public void Deserialize_AzureMetricConfigurationNotSuppliedWithNotLogAnalyticsResource_ReportsError()
        {
            const string yamlText =
                @"metrics:
                    name: 'test_metrics'
                    description: 'some metric'
                    resourceType: 'StorageAccount'";
            // Arrange
            var node = YamlUtils.CreateYamlNode(yamlText);
            var metricNode = (YamlMappingNode)node.Children["metrics"];

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                metricNode,
                "azureMetricConfiguration");
        }

        [Fact]
        public void Deserialize_AzureMetricConfigurationNotSuppliedWithLogAnalyticsResource_ReportsNoError()
        {
            const string yamlText =
                @"metrics:
                    name: 'test_metrics'
                    description: 'some metric'
                    resourceType: 'LogAnalytics'";
            // Arrange
            var node = YamlUtils.CreateYamlNode(yamlText);
            var metricNode = (YamlMappingNode)node.Children["metrics"];

            // Act / Assert
            YamlAssert.ReportsNoErrorForProperty(
                _deserializer,
                metricNode,
                "azureMetricConfiguration");
        }

        [Fact]
        public void Deserialize_LogAnalyticsNotSuppliedWithLogAnalyticsResource_ReportsError()
        {
            const string yamlText =
                @"metrics:
                    name: 'test_metrics'
                    description: 'some metric'
                    resourceType: 'LogAnalytics'";
            // Arrange
            var node = YamlUtils.CreateYamlNode(yamlText);
            var metricNode = (YamlMappingNode)node.Children["metrics"];

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                metricNode,
                "logAnalytics");
        }

        [Fact]
        public void Deserialize_ScrapingSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
@"scraping:
    interval: '00:05:00'";
            var node = YamlUtils.CreateYamlNode(yamlText);
            var scrapingNode = (YamlMappingNode)node.Children["scraping"];
            var scraping = new ScrapingV1();

            _scrapingDeserializer.Setup(d => d.Deserialize(scrapingNode, _errorReporter.Object)).Returns(scraping);

            // Act
            var definition = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Same(scraping, definition.Scraping);
        }

        [Fact]
        public void Deserialize_ScrapingNotSupplied_Null()
        {
            // Arrange
            const string yamlText = "name: promitor_test_metric";
            var node = YamlUtils.CreateYamlNode(yamlText);

            _scrapingDeserializer.Setup(
                d => d.Deserialize(It.IsAny<YamlMappingNode>(), It.IsAny<IErrorReporter>())).Returns(new ScrapingV1());

            // Act
            var definition = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Null(definition.Scraping);
        }

        [Fact]
        public void Deserialize_ResourcesSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
@"resourceType: Generic
resources:
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging-2";
            var node = YamlUtils.CreateYamlNode(yamlText);

            var resourceDeserializer = new Mock<IDeserializer<AzureResourceDefinitionV1>>();
            _resourceDeserializerFactory.Setup(
                f => f.GetDeserializerFor(ResourceType.Generic)).Returns(resourceDeserializer.Object);

            var resources = new List<AzureResourceDefinitionV1>
            {
                new() { ResourceGroupName = "promitor-group" }
            };
            resourceDeserializer.Setup(
                d => d.Deserialize((YamlSequenceNode)node.Children["resources"], _errorReporter.Object))
                .Returns(resources);

            // Act
            var definition = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Collection(definition.Resources,
                resource => Assert.Equal("promitor-group", resource.ResourceGroupName));
        }

        [Fact]
        public void Deserialize_ResourcesSupplied_DoesNotReportWarning()
        {
            // Because we're handling deserializing the resources manually, we
            // need to explicitly ignore the field to stop a warning being reported
            // about an unknown field

            // Arrange
            const string yamlText =
@"resourceType: Generic
resources:
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging-2";
            var node = YamlUtils.CreateYamlNode(yamlText);

            // Act
            _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            _errorReporter.Verify(
                r => r.ReportWarning(It.IsAny<YamlNode>(), It.Is<string>(s => s.Contains("resources"))), Times.Never);
        }

        [Fact]
        public void Deserialize_ResourcesWithUnspecifiedResourceType_Null()
        {
            // Arrange
            const string yamlText =
@"resources:
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging-2";
            var node = YamlUtils.CreateYamlNode(yamlText);

            var resourceDeserializer = new Mock<IDeserializer<AzureResourceDefinitionV1>>();
            _resourceDeserializerFactory.Setup(
                f => f.GetDeserializerFor(It.IsAny<ResourceType>())).Returns(resourceDeserializer.Object);

            resourceDeserializer.Setup(
                    d => d.Deserialize((YamlSequenceNode)node.Children["resources"], _errorReporter.Object))
                .Returns(new List<AzureResourceDefinitionV1>());

            // Act
            var definition = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Null(definition.Resources);
        }

        [Fact]
        public void Deserialize_ResourcesNotSupplied_Null()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceType: Generic");

            // Act
            var definition = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Null(definition.Resources);
        }

        [Fact]
        public void Deserialize_ResourceDiscoveryGroupsSupplied_DoesNotReportWarning()
        {
            // Because we're handling deserializing the resources manually, we
            // need to explicitly ignore the field to stop a warning being reported
            // about an unknown field

            // Arrange
            const string yamlText =
                @"resourceType: Generic
resourceDiscoveryGroups:
- name: sample-1
- name: sample-2";
            var node = YamlUtils.CreateYamlNode(yamlText);

            // Act
            _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            _errorReporter.Verify(
                r => r.ReportWarning(It.IsAny<YamlNode>(), It.Is<string>(s => s.Contains("resourceDiscoveryGroups"))), Times.Never);
        }

        [Fact]
        public void Deserialize_ResourceDiscoveryGroupsNotSupplied_Null()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceType: Generic");

            // Act
            var definition = _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            Assert.Null(definition.ResourceDiscoveryGroups);
        }

        [Fact]
        public void Deserialize_NoDeserializerForResourceType_ReportsError()
        {
            // Arrange
            const string yamlText =
@"resourceType: Generic
resources:
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging-2";
            var node = YamlUtils.CreateYamlNode(yamlText);

            _resourceDeserializerFactory.Setup(
                f => f.GetDeserializerFor(It.IsAny<ResourceType>())).Returns((IDeserializer<AzureResourceDefinitionV1>)null);

            // Act / Assert
            YamlAssert.ReportsError(
                _deserializer,
                node,
                node.Children["resourceType"],
                "Could not find a deserializer for resource type 'Generic'.");
        }

        [Fact]
        public void Deserialize_ResourcesAndCollectionNotSupplied_ReportsError()
        {
            // Either a static list of resources, or a resource collection must be
            // specified for the metric to be valid

            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceType: Generic");

            // Act
            _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            _errorReporter.Verify(
                reporter => reporter.ReportError(node, "Either 'resources' or 'resourceDiscoveryGroups' must be specified."));
        }

        [Fact]
        public void Deserialize_ResourcesSupplied_DoesNotReportError()
        {
            // Arrange
            const string yamlText =
@"resourceType: Generic
resources:
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging-2";
            var node = YamlUtils.CreateYamlNode(yamlText);

            SetupResourceDeserializer(node, new List<AzureResourceDefinitionV1> { new() });

            // Act
            _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            _errorReporter.Verify(
                reporter => reporter.ReportError(node, "Either 'resources' or 'resourceDiscoveryGroups' must be specified."), Times.Never);
        }

        [Fact]
        public void Deserialize_ResourcesEmpty_ReportsError()
        {
            // Arrange
            const string yamlText =
@"resourceType: Generic
resources: []";
            var node = YamlUtils.CreateYamlNode(yamlText);

            SetupResourceDeserializer(node, new List<AzureResourceDefinitionV1>());

            // Act
            _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            _errorReporter.Verify(
                reporter => reporter.ReportError(node, "Either 'resources' or 'resourceDiscoveryGroups' must be specified."));
        }

        [Fact]
        public void Deserialize_ResourceDiscoveryGroupsSupplied_DoesNotReportError()
        {
            // Arrange
            const string yamlText =
@"resourceType: Generic
resourceDiscoveryGroups:
- name: orders";
            var node = YamlUtils.CreateYamlNode(yamlText);
            var resourceDiscoveryGroups = new List<AzureResourceDiscoveryGroupDefinitionV1>
            {
                new()
            };
            _resourceDiscoveryGroupDeserializer.Setup(
                d => d.Deserialize(It.IsAny<YamlSequenceNode>(), It.IsAny<IErrorReporter>())).Returns(resourceDiscoveryGroups);

            // Act
            _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            _errorReporter.Verify(
                reporter => reporter.ReportError(node, "Either 'resources' or 'resourceDiscoveryGroups' must be specified."), Times.Never);
        }

        [Fact]
        public void Deserialize_ResourceDiscoveryGroupsEmpty_ReportsError()
        {
            // Arrange
            const string yamlText =
@"resourceType: Generic
resourceDiscoveryGroups: []";
            var node = YamlUtils.CreateYamlNode(yamlText);
            _resourceDiscoveryGroupDeserializer.Setup(
                d => d.Deserialize(It.IsAny<YamlSequenceNode>(), It.IsAny<IErrorReporter>())).Returns(new List<AzureResourceDiscoveryGroupDefinitionV1>());

            // Act
            _deserializer.Deserialize(node, _errorReporter.Object);

            // Assert
            _errorReporter.Verify(
                reporter => reporter.ReportError(node, "Either 'resources' or 'resourceDiscoveryGroups' must be specified."));
        }

        private void SetupResourceDeserializer(YamlMappingNode node, IReadOnlyCollection<AzureResourceDefinitionV1> resources)
        {
            var resourceDeserializer = new Mock<IDeserializer<AzureResourceDefinitionV1>>();
            _resourceDeserializerFactory.Setup(
                f => f.GetDeserializerFor(It.IsAny<ResourceType>())).Returns(resourceDeserializer.Object);

            resourceDeserializer.Setup(
                    d => d.Deserialize((YamlSequenceNode)node.Children["resources"], _errorReporter.Object))
                .Returns(resources);
        }
    }
}
