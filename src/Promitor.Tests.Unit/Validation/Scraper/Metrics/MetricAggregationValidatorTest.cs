using System;
using System.ComponentModel;
using Promitor.Agents.Scraper.Validation.MetricDefinitions;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Model;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Scraper.Metrics;

[Category("Unit")]
public class MetricAggregationValidatorTest
{
    [Theory]
    [InlineData("00:05:00", "00:00:00")]
    [InlineData("00:05:00", "00:05:00")]
    [InlineData("00:05:00", "00:10:00")]
    [InlineData("00:05:00", "02:00:00")]
    [InlineData("00:01:00", "00:02:00")]
    [InlineData("00:15:00", "00:30:00")]
    public void Validate_OffsetIsMultipleOfAggregationInterval_Succeeds(string interval, string offset)
    {
        // Arrange
        var aggregation = CreateAggregation(TimeSpan.Parse(interval), TimeSpan.Parse(offset));
        var validator = new MetricAggregationValidator(new MetricDefaults());

        // Act
        var validationErrors = validator.Validate(aggregation);

        // Assert
        Assert.Empty(validationErrors);
    }

    [Fact]
    public void Validate_NoOffset_Succeeds()
    {
        // Arrange
        var aggregation = CreateAggregation(TimeSpan.FromMinutes(5), offset: null);
        var validator = new MetricAggregationValidator(new MetricDefaults());

        // Act
        var validationErrors = validator.Validate(aggregation);

        // Assert
        Assert.Empty(validationErrors);
    }

    [Theory]
    [InlineData("00:05:00", "00:01:00")]
    [InlineData("00:05:00", "00:07:00")]
    [InlineData("00:05:00", "00:05:30")]
    [InlineData("00:15:00", "00:10:00")]
    [InlineData("00:01:00", "00:01:30")]
    public void Validate_OffsetIsNotMultipleOfAggregationInterval_Fails(string interval, string offset)
    {
        // Arrange
        var aggregation = CreateAggregation(TimeSpan.Parse(interval), TimeSpan.Parse(offset));
        var validator = new MetricAggregationValidator(new MetricDefaults());

        // Act
        var validationErrors = validator.Validate(aggregation);

        // Assert
        Assert.Single(validationErrors);
    }

    [Fact]
    public void Validate_OffsetWithoutMetricInterval_UsesDefaultAggregationInterval()
    {
        // Arrange
        var aggregation = CreateAggregation(interval: null, TimeSpan.FromMinutes(10));
        var metricDefaults = new MetricDefaults { Aggregation = new Aggregation { Interval = TimeSpan.FromMinutes(15) } };
        var validator = new MetricAggregationValidator(metricDefaults);

        // Act
        var validationErrors = validator.Validate(aggregation);

        // Assert
        Assert.Single(validationErrors);
    }

    [Fact]
    public void Validate_NegativeOffset_Fails()
    {
        // Arrange
        var aggregation = CreateAggregation(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(-5));
        var validator = new MetricAggregationValidator(new MetricDefaults());

        // Act
        var validationErrors = validator.Validate(aggregation);

        // Assert
        Assert.Single(validationErrors);
    }

    private static MetricAggregation CreateAggregation(TimeSpan? interval, TimeSpan? offset)
    {
        return new MetricAggregation
        {
            Type = PromitorMetricAggregationType.Average,
            Interval = interval,
            Offset = offset
        };
    }
}
