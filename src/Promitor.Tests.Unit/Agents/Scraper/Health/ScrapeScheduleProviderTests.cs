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
        public void GetMinimumScrapeInterval_MetricsWithSameSchedule_ReturnsExpectedInterval()
        {
            // Arrange
            var metrics = CreateMetricsDeclaration(
                ("0 */2 * * * *", "metric1"), // Every 2 minutes
                ("0 */2 * * * *", "metric2")  // Every 2 minutes
            );

            var mockProvider = CreateMockMetricsProvider(metrics);
            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(2), result);
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
        public void GetMinimumScrapeInterval_NullMetricsDeclaration_ReturnsDefaultOf5Minutes()
        {
            // Arrange
            var mockProvider = new Mock<IMetricsDeclarationProvider>();
            mockProvider.Setup(m => m.Get(true, null)).Returns((MetricsDeclaration)null);

            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(5), result);
        }

        [Fact]
        public void GetMinimumScrapeInterval_MetricsWithNullSchedules_ReturnsDefaultOf5Minutes()
        {
            // Arrange
            var metrics = new MetricsDeclaration
            {
                AzureMetadata = new AzureMetadata(),
                MetricDefaults = new MetricDefaults(),
                Metrics = new System.Collections.Generic.List<MetricDefinition>
                {
                    new MetricDefinition
                    {
                        Scraping = null
                    }
                }
            };

            var mockProvider = CreateMockMetricsProvider(metrics);
            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(5), result);
        }

        [Fact]
        public void GetMinimumScrapeInterval_MetricsWithEmptySchedules_ReturnsDefaultOf5Minutes()
        {
            // Arrange
            var metrics = new MetricsDeclaration
            {
                AzureMetadata = new AzureMetadata(),
                MetricDefaults = new MetricDefaults(),
                Metrics = new System.Collections.Generic.List<MetricDefinition>
                {
                    new MetricDefinition
                    {
                        Scraping = new Scraping { Schedule = "" }
                    },
                    new MetricDefinition
                    {
                        Scraping = new Scraping { Schedule = "   " }
                    }
                }
            };

            var mockProvider = CreateMockMetricsProvider(metrics);
            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(5), result);
        }

        [Fact]
        public void GetMinimumScrapeInterval_CalledMultipleTimes_UsesCachedValue()
        {
            // Arrange
            var metrics = CreateMetricsDeclaration(
                ("0 * * * * *", "metric1") // Every minute
            );

            var mockProvider = CreateMockMetricsProvider(metrics);
            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result1 = scheduleProvider.GetMinimumScrapeInterval();
            var result2 = scheduleProvider.GetMinimumScrapeInterval();
            var result3 = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(1), result1);
            Assert.Equal(TimeSpan.FromMinutes(1), result2);
            Assert.Equal(TimeSpan.FromMinutes(1), result3);

            // Verify Get was only called once (cached)
            mockProvider.Verify(m => m.Get(true, null), Times.Once);
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

        [Fact]
        public void GetMinimumScrapeInterval_Every10Seconds_Returns10Seconds()
        {
            // Arrange
            var metrics = CreateMetricsDeclaration(
                ("*/10 * * * * *", "metric1") // Every 10 seconds
            );

            var mockProvider = CreateMockMetricsProvider(metrics);
            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromSeconds(10), result);
        }

        [Fact]
        public void GetMinimumScrapeInterval_MixOfValidAndInvalidSchedules_ReturnsMinimumOfValidOnes()
        {
            // Arrange
            var metrics = new MetricsDeclaration
            {
                AzureMetadata = new AzureMetadata(),
                MetricDefaults = new MetricDefaults(),
                Metrics = new System.Collections.Generic.List<MetricDefinition>
                {
                    new MetricDefinition
                    {
                        Scraping = new Scraping { Schedule = "0 * * * * *" } // Valid: every minute
                    },
                    new MetricDefinition
                    {
                        Scraping = new Scraping { Schedule = "invalid cron" } // Invalid
                    },
                    new MetricDefinition
                    {
                        Scraping = new Scraping { Schedule = "0 */5 * * * *" } // Valid: every 5 minutes
                    }
                }
            };

            var mockProvider = CreateMockMetricsProvider(metrics);
            var scheduleProvider = new ScrapeScheduleProvider(mockProvider.Object, NullLogger<ScrapeScheduleProvider>.Instance);

            // Act
            var result = scheduleProvider.GetMinimumScrapeInterval();

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(1), result);
        }

        [Fact]
        public void Constructor_NullMetricsDeclarationProvider_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ScrapeScheduleProvider(null, NullLogger<ScrapeScheduleProvider>.Instance));
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange
            var mockProvider = new Mock<IMetricsDeclarationProvider>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ScrapeScheduleProvider(mockProvider.Object, null));
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

