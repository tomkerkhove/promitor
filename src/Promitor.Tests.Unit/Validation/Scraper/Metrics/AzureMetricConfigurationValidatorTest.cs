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
        };

        var metricDefinition = new MetricDefinition
        {
            AzureMetricConfiguration = metricConfig
        };

        metricConfig.Dimensions = new List<MetricDimension> { new(), new() };
        metricConfig.Dimension = new MetricDimension();

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
        };

        var metricDefinition = new MetricDefinition
        {
            AzureMetricConfiguration = metricConfig
        };

        metricConfig.Dimension = new MetricDimension();
        metricConfig.Dimensions = new List<MetricDimension>();

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
        };

        var metricDefinition = new MetricDefinition
        {
            AzureMetricConfiguration = metricConfig
        };

        metricConfig.Dimensions = new List<MetricDimension> { new(), new() };

        // Act
        var azureMetricConfigurationValidator = new AzureMetricConfigurationValidator( new MetricDefaults());
        var validationErrors = azureMetricConfigurationValidator.Validate(metricDefinition);

        // Assert
        Assert.Empty(validationErrors);
    }
}