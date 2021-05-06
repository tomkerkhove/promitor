using System;
using System.ComponentModel;
using System.Threading.Tasks;
using JustEat.StatsD;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Metrics;
using Promitor.Integrations.Sinks.Statsd;
using Promitor.Tests.Unit.Generators;
using Xunit;

namespace Promitor.Tests.Unit.Metrics.Sinks
{
    [Category("Unit")]
    public class StatsDMetricSinkTests : UnitTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ReportMetricAsync_InputDoesNotContainMetricName_ThrowsException(string metricName)
        {
            // Arrange
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var metricValue = BogusGenerator.Random.Double();
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var statsDPublisherMock = new Mock<IStatsDPublisher>();
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, NullLogger<StatsdMetricSink>.Instance);

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            await Assert.ThrowsAsync<ArgumentException>(() => metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ReportMetricAsync_InputDoesNotContainMetricDescription_Succeeds(string metricDescription)
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricValue = BogusGenerator.Random.Double();
            var measuredMetric = MeasuredMetric.CreateWithoutDimension(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var statsDPublisherMock = new Mock<IStatsDPublisher>();
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, NullLogger<StatsdMetricSink>.Instance);

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);
        }

        [Fact]
        public async Task ReportMetricAsync_InputDoesNotContainMeasuredMetric_ThrowsException()
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var statsDPublisherMock = new Mock<IStatsDPublisher>();
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, NullLogger<StatsdMetricSink>.Instance);

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            await Assert.ThrowsAsync<ArgumentNullException>(() => metricSink.ReportMetricAsync(metricName, metricDescription, null));
        }

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithMetricValue_SuccessfullyWritesMetric()
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var metricValue = BogusGenerator.Random.Double();
            var measuredMetric = MeasuredMetric.CreateWithoutDimension(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var statsDPublisherMock = new Mock<IStatsDPublisher>();
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, NullLogger<StatsdMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            statsDPublisherMock.Verify(mock => mock.Gauge(metricValue, metricName), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithoutMetricValue_SuccessfullyWritesMetricWithDefault()
        {
            // Arrange
            const double expectedDefaultValue = 0;
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            double? metricValue = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var measuredMetric = MeasuredMetric.CreateWithoutDimension(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var statsDPublisherMock = new Mock<IStatsDPublisher>();
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, NullLogger<StatsdMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            statsDPublisherMock.Verify(mock => mock.Gauge(expectedDefaultValue, metricName), Times.Once());
        }
    }
}