using System.Collections.Generic;
using Promitor.Agents.Scraper.Validation.MetricDefinitions;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Xunit;
using MetricDimension = Promitor.Core.Scraping.Configuration.Model.MetricDimension;

namespace Promitor.Tests.Unit.Validation.Scraper.Metrics;

public class AzureMetricConfigurationValidatorTest
{
    [Fact]
    public void DimensionsAndDimension_Fails()
    {
        // Arrange
        var metricConfig = new AzureMetricConfiguration
        {
            MetricName = "testMetric",
            Dimension = new MetricDimension { Name = "testDimension1" },
            Dimensions = new List<MetricDimension> { new() {Name = "testDimension2"}, new() { Name = "testDimension3" } }
        };

        var metricDefinition = new MetricDefinition
        {
            AzureMetricConfiguration = metricConfig
        };

        // Act
        var azureMetricConfigurationValidator = new AzureMetricConfigurationValidator( new MetricDefaults());
        var validationErrors = azureMetricConfigurationValidator.Validate(metricDefinition);

        // Assert
        Assert.NotEmpty(validationErrors);
    }

    [Fact]
    public void OnlyDimension_Succeeds()
    {
        // Arrange
        var metricConfig = new AzureMetricConfiguration
        {
            MetricName = "testMetric",
            Dimension = new MetricDimension { Name = "testDimension1" },
            Dimensions = new List<MetricDimension>()
        };

        var metricDefinition = new MetricDefinition
        {
            AzureMetricConfiguration = metricConfig
        };

        // Act
        var azureMetricConfigurationValidator = new AzureMetricConfigurationValidator( new MetricDefaults());
        var validationErrors = azureMetricConfigurationValidator.Validate(metricDefinition);

        // Assert
        Assert.Empty(validationErrors);
    }

    [Fact]
    public void OnlyDimensions_Succeeds()
    {
        // Arrange
        var metricConfig = new AzureMetricConfiguration
        {
            MetricName = "testMetric",
            Dimensions = new List<MetricDimension> { new() {Name = "testDimension1"}, new() { Name = "testDimension2" } }
        };

        var metricDefinition = new MetricDefinition
        {
            AzureMetricConfiguration = metricConfig
        };

        // Act
        var azureMetricConfigurationValidator = new AzureMetricConfigurationValidator( new MetricDefaults());
        var validationErrors = azureMetricConfigurationValidator.Validate(metricDefinition);

        // Assert
        Assert.Empty(validationErrors);
    }
}