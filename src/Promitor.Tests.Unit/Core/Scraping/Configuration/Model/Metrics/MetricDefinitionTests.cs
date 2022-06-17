using System.Collections.Generic;
using System.ComponentModel;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Xunit;

namespace Promitor.Tests.Unit.Core.Scraping.Configuration.Model.Metrics
{
    [Category("Unit")]
    public class MetricDefinitionTests
    {
        private readonly PrometheusMetricDefinition _prometheusMetricDefinition =
            new PrometheusMetricDefinition("promitor_test", "test", new Dictionary<string, string>());

        private readonly AzureMetadata _azureMetadata = new AzureMetadata { ResourceGroupName = "global-resource-group", SubscriptionId = "global-subscription-id"};

        [Fact]
        public void CreateScrapeDefinition_ResourceOverridesResourceGroupName_UsesOverriddenName()
        {
            // Arrange
            var resource = new ContainerInstanceResourceDefinition(null, "containerInstanceResourceGroup", "containerGroup");
            var definition = new MetricDefinition(_prometheusMetricDefinition, new Promitor.Core.Scraping.Configuration.Model.Scraping(), new AzureMetricConfiguration(), ResourceType.ContainerInstance, new List<AzureResourceDefinition> { resource });

            // Act
            var scrapeDefinition = definition.CreateScrapeDefinition(resource, _azureMetadata);

            // Assert
            Assert.Equal(resource.ResourceGroupName, scrapeDefinition.ResourceGroupName);
        }

        [Fact]
        public void CreateScrapeDefinition_ResourceDoesNotSpecifyResourceGroupName_UsesGlobalName()
        {
            // Arrange
            var resource = new ContainerInstanceResourceDefinition("subscription", null, "containerGroup");
            var definition = new MetricDefinition(_prometheusMetricDefinition, new Promitor.Core.Scraping.Configuration.Model.Scraping(), new AzureMetricConfiguration(), ResourceType.ContainerInstance, new List<AzureResourceDefinition> { resource });

            // Act
            var scrapeDefinition = definition.CreateScrapeDefinition(resource, _azureMetadata);

            // Assert
            Assert.Equal(_azureMetadata.ResourceGroupName, scrapeDefinition.ResourceGroupName);
        }
        [Fact]
        public void CreateScrapeDefinition_ResourceOverridesSubscription_UsesOverriddenName()
        {
            // Arrange
            var resource = new ContainerInstanceResourceDefinition("subscription", "containerInstanceResourceGroup", "containerGroup");
            var definition = new MetricDefinition(_prometheusMetricDefinition, new Promitor.Core.Scraping.Configuration.Model.Scraping(), new AzureMetricConfiguration(), ResourceType.ContainerInstance, new List<AzureResourceDefinition> { resource });

            // Act
            var scrapeDefinition = definition.CreateScrapeDefinition(resource, _azureMetadata);

            // Assert
            Assert.Equal(resource.SubscriptionId, scrapeDefinition.SubscriptionId);
        }

        [Fact]
        public void CreateScrapeDefinition_ResourceDoesNotSpecifySubscription_UsesGlobalName()
        {
            // Arrange
            var resource = new ContainerInstanceResourceDefinition(null, "containerInstanceResourceGroup", "containerGroup");
            var definition = new MetricDefinition(_prometheusMetricDefinition, new Promitor.Core.Scraping.Configuration.Model.Scraping(), new AzureMetricConfiguration(), ResourceType.ContainerInstance, new List<AzureResourceDefinition> { resource });

            // Act
            var scrapeDefinition = definition.CreateScrapeDefinition(resource, _azureMetadata);

            // Assert
            Assert.Equal(_azureMetadata.SubscriptionId, scrapeDefinition.SubscriptionId);
        }

        [Fact]
        public void CreateScrapeDefinition_ResourceHasEmptyResourceGroupName_UsesGlobalName()
        {
            // Arrange
            var resource = new ContainerInstanceResourceDefinition("subscription", string.Empty, "containerGroup");
            var definition = new MetricDefinition(_prometheusMetricDefinition, new Promitor.Core.Scraping.Configuration.Model.Scraping(), new AzureMetricConfiguration(), ResourceType.ContainerInstance, new List<AzureResourceDefinition> { resource });

            // Act
            var scrapeDefinition = definition.CreateScrapeDefinition(resource, _azureMetadata);

            // Assert
            Assert.Equal(_azureMetadata.ResourceGroupName, scrapeDefinition.ResourceGroupName);
        }

        [Fact]
        public void CreateScrapeDefinition_MetricDimensionIsEquatable_UsesValueComparison()
        {
            // Arrange
            var dimension1 = new MetricDimension { Name = "MyMetricDimension" };
            var dimension2 = new MetricDimension { Name = dimension1.Name };

            // Assert
            Assert.Equal(dimension1, dimension2);
        }

        [Fact]
        public void CreateScrapeDefinition_MetricDimensionIsDistinguishable_UsesValueComparison()
        {
            // Arrange
            var dimension1 = new MetricDimension { Name = "MetricDimension1" };
            var dimension2 = new MetricDimension { Name = "MetricDimension2" };

            // Assert
            Assert.NotEqual(dimension1, dimension2);
        }

        [Fact]
        public void CreateScrapeDefinition_ScrapingIsEquatable_UsesValueComparison()
        {
            // Arrange
            var scraping1 = new Promitor.Core.Scraping.Configuration.Model.Scraping { Schedule = "5 * * * *" };
            var scraping2 = new Promitor.Core.Scraping.Configuration.Model.Scraping { Schedule = scraping1.Schedule };

            // Assert
            Assert.Equal(scraping1, scraping2);
        }

        [Fact]
        public void CreateScrapeDefinition_ScrapingIsDistinguishable_UsesValueComparison()
        {
            // Arrange
            var scraping1 = new Promitor.Core.Scraping.Configuration.Model.Scraping { Schedule = "5 * * * *" };
            var scraping2 = new Promitor.Core.Scraping.Configuration.Model.Scraping { Schedule = "6 * * * *" };

            // Assert
            Assert.NotEqual(scraping1, scraping2);
        }
    }
}
