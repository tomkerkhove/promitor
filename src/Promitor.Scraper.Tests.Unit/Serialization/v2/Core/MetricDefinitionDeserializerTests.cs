﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Core;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Core
{
    [Category("Unit")]
    public class MetricDefinitionDeserializerTests
    {
        private readonly Mock<IDeserializer<AzureMetricConfigurationV2>> _azureMetricConfigurationDeserializer;
        private readonly Mock<IDeserializer<ScrapingV2>> _scrapingDeserializer;
        private readonly Mock<IAzureResourceDeserializerFactory> _resourceDeserializerFactory;

        private readonly MetricDefinitionDeserializer _deserializer;

        public MetricDefinitionDeserializerTests()
        {
            _azureMetricConfigurationDeserializer = new Mock<IDeserializer<AzureMetricConfigurationV2>>();
            _scrapingDeserializer = new Mock<IDeserializer<ScrapingV2>>();
            _resourceDeserializerFactory = new Mock<IAzureResourceDeserializerFactory>();

            _deserializer = new MetricDefinitionDeserializer(
                _azureMetricConfigurationDeserializer.Object,
                _scrapingDeserializer.Object,
                _resourceDeserializerFactory.Object,
                new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_NameSupplied_SetsName()
        {
            AssertPropertySet(
                "name: promitor_test_metric",
                "promitor_test_metric",
                d => d.Name);
        }

        [Fact]
        public void Deserialize_NameNotSupplied_Null()
        {
            AssertPropertyNull("description: 'Test metric'", d => d.Name);
        }

        [Fact]
        public void Deserialize_DescriptionSupplied_SetsDescription()
        {
            AssertPropertySet(
                "description: 'This is a test metric'",
                "This is a test metric",
                d => d.Description);
        }

        [Fact]
        public void Deserialize_DescriptionNotSupplied_Null()
        {
            AssertPropertyNull("name: metric", d => d.Description);
        }

        [Fact]
        public void Deserialize_ResourceTypeSupplied_SetsResourceType()
        {
            AssertPropertySet(
                "resourceType: ServiceBusQueue",
                ResourceType.ServiceBusQueue,
                d => d.ResourceType);
        }

        [Fact]
        public void Deserialize_ResourceTypeNotSupplied_Defaults()
        {
            AssertPropertySet(
                "name: promitor_test_metric",
                ResourceType.NotSpecified,
                d => d.ResourceType);
        }

        [Fact]
        public void Deserialize_LabelsSupplied_SetsLabels()
        {
            const string yamlText =
@"labels:
    app: promitor
    env: test";

            AssertPropertySet(
                yamlText,
                new Dictionary<string, string>{{"app", "promitor"}, {"env", "test"}},
                d => d.Labels);
        }

        [Fact]
        public void Deserialize_LabelsNotSupplied_Null()
        {
            AssertPropertyNull("name: promitor_test_metric", d => d.Labels);
        }

        [Fact]
        public void Deserialize_AzureMetricConfigurationSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
@"azureMetricConfiguration:
    metricName: ActiveMessages";
            var node = YamlUtils.CreateYamlNode(yamlText);
            var configurationNode = (YamlMappingNode) node.Children["azureMetricConfiguration"];
            var configuration = new AzureMetricConfigurationV2();

            _azureMetricConfigurationDeserializer.Setup(d => d.Deserialize(configurationNode)).Returns(configuration);

            // Act
            var definition = _deserializer.Deserialize(node);

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
                d => d.Deserialize(It.IsAny<YamlMappingNode>())).Returns(new AzureMetricConfigurationV2());

            // Act
            var definition = _deserializer.Deserialize(node);

            // Assert
            Assert.Null(definition.AzureMetricConfiguration);
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
            var scraping = new ScrapingV2();

            _scrapingDeserializer.Setup(d => d.Deserialize(scrapingNode)).Returns(scraping);

            // Act
            var definition = _deserializer.Deserialize(node);

            // Assert
            Assert.Same(scraping, definition.Scraping);
        }

        [Fact]
        public void Deserialize_ScrapingNotSupplied_Null()
        {
            // Arrange
            const string yamlText = "name: promitor_test_metric";
            var node = YamlUtils.CreateYamlNode(yamlText);

            _scrapingDeserializer.Setup(d => d.Deserialize(It.IsAny<YamlMappingNode>())).Returns(new ScrapingV2());

            // Act
            var definition = _deserializer.Deserialize(node);

            // Assert
            Assert.Null(definition.Scraping);
        }

        [Fact]
        public void Deserialize_ResourcesSupplied_UsesDeserializer()
        {
            // Arrange
            const string yamlText =
@"resourceType: Generic
metrics:
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging-2";
            var node = YamlUtils.CreateYamlNode(yamlText);

            var resourceDeserializer = new Mock<IDeserializer<AzureResourceDefinitionV2>>();
            _resourceDeserializerFactory.Setup(
                f => f.GetDeserializerFor(ResourceType.Generic)).Returns(resourceDeserializer.Object);

            var resources = new List<AzureResourceDefinitionV2>();
            resourceDeserializer.Setup(
                d => d.Deserialize((YamlSequenceNode) node.Children["metrics"])).Returns(resources);

            // Act
            var definition = _deserializer.Deserialize(node);

            // Assert
            Assert.Same(resources, definition.Resources);
        }

        [Fact]
        public void Deserialize_ResourcesWithUnspecifiedResourceType_Null()
        {
            // Arrange
            const string yamlText =
@"metrics:
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging
- resourceUri: Microsoft.ServiceBus/namespaces/promitor-messaging-2";
            var node = YamlUtils.CreateYamlNode(yamlText);

            var resourceDeserializer = new Mock<IDeserializer<AzureResourceDefinitionV2>>();
            _resourceDeserializerFactory.Setup(
                f => f.GetDeserializerFor(It.IsAny<ResourceType>())).Returns(resourceDeserializer.Object);

            var resources = new List<AzureResourceDefinitionV2>();
            resourceDeserializer.Setup(
                d => d.Deserialize((YamlSequenceNode)node.Children["metrics"])).Returns(resources);

            // Act
            var definition = _deserializer.Deserialize(node);

            // Assert
            Assert.Null(definition.Resources);
        }

        private void AssertPropertySet<T>(string yamlText, T expected, Func<MetricDefinitionV2, T> propertyAccessor)
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(yamlText);

            // Act
            var definition = _deserializer.Deserialize(node);

            // Assert
            Assert.Equal(expected, propertyAccessor(definition));
        }

        private void AssertPropertyNull<T>(string yamlText, Func<MetricDefinitionV2, T> propertyAccessor)
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode(yamlText);

            // Act
            var definition = _deserializer.Deserialize(node);

            // Assert
            Assert.Null(propertyAccessor(definition));
        }
    }
}
