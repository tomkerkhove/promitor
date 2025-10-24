using System;
using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Agents.Scraper.Health;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Xunit;

namespace Promitor.Tests.Unit.Agents.Scraper.Health
{
    [Category("Unit")]
    public class ScrapeScheduleProviderTests : UnitTest
    {
        [Fact]
        public void GetMinimumScrapeInterval_SingleMetricWith1MinuteSchedule_Returns1Minute()
        {
            // Arrange
            var metrics = CreateMetricsDeclaration(
                ("0 * * * * *", "metric1") // Every minute
            );

            var mockProvider = CreateMockMetricsProvider(metrics);
            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(1), result);
        }

        [Fact]
        public void GetMinimumScrapeInterval_MultipleMetricsWithDifferentSchedules_ReturnsMinimum()
        {
            // Arrange
            var metrics = CreateMetricsDeclaration(
                ("0 * * * * *", "metric1"),    // Every minute
                ("*/30 * * * * *", "metric2"), // Every 30 seconds
                ("0 */5 * * * *", "metric3")   // Every 5 minutes
            );

            var mockProvider = CreateMockMetricsProvider(metrics);
            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromSeconds(30), result);
        }

        [Fact]
        public void GetMinimumScrapeInterval_NoMetrics_ReturnsDefaultOf5Minutes()
        {
            // Arrange
            var metrics = new MetricsDeclaration
            {
                AzureMetadata = new AzureMetadata(),
                MetricDefaults = new MetricDefaults(),
                Metrics = new System.Collections.Generic.List<MetricDefinition>()
            };

            var mockProvider = CreateMockMetricsProvider(metrics);
            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(5), result);
        }

        [Fact]
        public void GetMinimumScrapeInterval_DailySchedule_ReturnsOneDay()
        {
            // Arrange
            var metrics = CreateMetricsDeclaration(
                ("0 0 12 * * *", "metric1") // Every day at noon
            );

            var mockProvider = CreateMockMetricsProvider(metrics);
            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromDays(1), result);
        }

        [Fact]
        public void GetMinimumScrapeInterval_HourlySchedule_ReturnsOneHour()
        {
            // Arrange
            var metrics = CreateMetricsDeclaration(
                ("0 0 * * * *", "metric1") // Every hour
            );

            var mockProvider = CreateMockMetricsProvider(metrics);
            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromHours(1), result);
        }

        private MetricsDeclaration CreateMetricsDeclaration(params (string cronSchedule, string metricId)[] metrics)
        {
            var metricsList = new System.Collections.Generic.List<MetricDefinition>();

            foreach (var (cronSchedule, _) in metrics)
            {
                metricsList.Add(new MetricDefinition
                {
                    Scraping = new Scraping
                    {
                        Schedule = cronSchedule
                    }
                });
            }

            return new MetricsDeclaration
            {
                AzureMetadata = new AzureMetadata(),
                MetricDefaults = new MetricDefaults(),
                Metrics = metricsList
            };
        }

        private Mock<IMetricsDeclarationProvider> CreateMockMetricsProvider(MetricsDeclaration metricsDeclaration)
        {
            var mockProvider = new Mock<IMetricsDeclarationProvider>();
            mockProvider.Setup(m => m.Get(true, null)).Returns(metricsDeclaration);
            return mockProvider;
        }
    }
}

