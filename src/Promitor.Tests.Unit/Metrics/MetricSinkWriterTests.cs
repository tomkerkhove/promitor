using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Bogus;
using JustEat.StatsD;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.Sinks.Statsd;
using Promitor.Tests.Unit.Generators;
using Xunit;

namespace Promitor.Tests.Unit.Metrics
{
    [Category("Unit")]
    public class MetricSinkWriterTests
    {
        private readonly Faker _bogus = new Faker();
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ReportMetricAsync_WriteToOneSinkWithoutMetricName_ThrowsException(string metricName)
        {
            // Arrange
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var metricSink = new Mock<IMetricSink>();
            var metricSinkWriter = new MetricSinkWriter(new List<IMetricSink> { metricSink.Object }, NullLogger<MetricSinkWriter>.Instance);

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            await Assert.ThrowsAsync<ArgumentException>(() => metricSinkWriter.ReportMetricAsync(metricName, metricDescription, scrapeResult));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ReportMetricAsync_WriteToOneSinkWithoutMetricDescription_Succeeds(string metricDescription)
        {
            // Arrange
            var metricName = _bogus.Name.FirstName();
            var metricValue = _bogus.Random.Double();
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var metricSink = new Mock<IMetricSink>();
            var metricSinkWriter = new MetricSinkWriter(new List<IMetricSink> { metricSink.Object }, NullLogger<MetricSinkWriter>.Instance);

            // Act & Assert
            await metricSinkWriter.ReportMetricAsync(metricName, metricDescription, scrapeResult);
        }

        [Fact]
        public async Task ReportMetricAsync_WriteToOneSinkWithoutScrapeResult_ThrowsException()
        {
            // Arrange
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            ScrapeResult scrapeResult = null;
            var metricSink = new Mock<IMetricSink>();
            var metricSinkWriter = new MetricSinkWriter(new List<IMetricSink> { metricSink.Object }, NullLogger<MetricSinkWriter>.Instance);

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            await Assert.ThrowsAsync<ArgumentNullException>(() => metricSinkWriter.ReportMetricAsync(metricName, metricDescription, scrapeResult));
        }

        [Fact]
        public async Task ReportMetricAsync_WriteToNoSinks_Succeeds()
        {
            // Arrange
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var metricSinkWriter = new MetricSinkWriter(new List<IMetricSink>(), NullLogger<MetricSinkWriter>.Instance);

            // Act & Assert
            await metricSinkWriter.ReportMetricAsync(metricName, metricDescription, scrapeResult);
        }

        [Fact]
        public async Task ReportMetricAsync_WriteToOneSink_Succeeds()
        {
            // Arrange
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var metricSink = new Mock<IMetricSink>();
            var metricSinkWriter = new MetricSinkWriter(new List<IMetricSink> { metricSink.Object }, NullLogger<MetricSinkWriter>.Instance);

            // Act
            await metricSinkWriter.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            metricSink.Verify(mock => mock.ReportMetricAsync(metricName, metricDescription, scrapeResult), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_WriteToMultipleSinks_Succeeds()
        {
            // Arrange
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var firstSink = new Mock<IMetricSink>();
            var secondSink = new Mock<IMetricSink>();
            var metricSinkWriter = new MetricSinkWriter(new List<IMetricSink> { firstSink.Object, secondSink.Object }, NullLogger<MetricSinkWriter>.Instance);

            // Act
            await metricSinkWriter.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            firstSink.Verify(mock => mock.ReportMetricAsync(metricName, metricDescription, scrapeResult), Times.Once());
            secondSink.Verify(mock => mock.ReportMetricAsync(metricName, metricDescription, scrapeResult), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_WriteToStatsDSink_Succeeds()
        {
            // Arrange
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var statsDPublisherMock = new Mock<IStatsDPublisher>();
            var statsdMetricSink = new StatsdMetricSink(statsDPublisherMock.Object, NullLogger<StatsdMetricSink>.Instance);
            var metricSinkWriter = new MetricSinkWriter(new List<IMetricSink> { statsdMetricSink }, NullLogger<MetricSinkWriter>.Instance);
            
            // Act
            await metricSinkWriter.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            statsDPublisherMock.Verify(mock => mock.Gauge(metricValue, metricName), Times.Once());
        }
    }
}