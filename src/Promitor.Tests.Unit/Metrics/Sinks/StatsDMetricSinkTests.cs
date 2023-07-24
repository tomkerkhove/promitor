using System;
using System.ComponentModel;
using System.Threading.Tasks;
using JustEat.StatsD;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Promitor.Core.Metrics;
using Promitor.Integrations.Sinks.Statsd;
using Promitor.Integrations.Sinks.Statsd.Configuration;
using Promitor.Tests.Unit.Generators;
using Promitor.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Tests.Unit.Metrics.Sinks
{
    [Category("Unit")]
    public class StatsDMetricSinkTests : MetricSinkTest
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
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var statsDSinkConfiguration = CreateStatsDConfiguration();
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, metricsDeclarationProvider, statsDSinkConfiguration, NullLogger<StatsdMetricSink>.Instance);

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
            var measuredMetric = MeasuredMetric.CreateWithoutDimensions(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var statsDPublisherMock = new Mock<IStatsDPublisher>();
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var statsDSinkConfiguration = CreateStatsDConfiguration();
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, metricsDeclarationProvider, statsDSinkConfiguration, NullLogger<StatsdMetricSink>.Instance);

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
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var statsDSinkConfiguration = CreateStatsDConfiguration();
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, metricsDeclarationProvider, statsDSinkConfiguration, NullLogger<StatsdMetricSink>.Instance);

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
            var measuredMetric = MeasuredMetric.CreateWithoutDimensions(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var statsDPublisherMock = new Mock<IStatsDPublisher>();
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var statsDSinkConfiguration = CreateStatsDConfiguration();
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, metricsDeclarationProvider, statsDSinkConfiguration, NullLogger<StatsdMetricSink>.Instance);

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
            var measuredMetric = MeasuredMetric.CreateWithoutDimensions(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var statsDPublisherMock = new Mock<IStatsDPublisher>();
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var statsDSinkConfiguration = CreateStatsDConfiguration();
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, metricsDeclarationProvider, statsDSinkConfiguration, NullLogger<StatsdMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            statsDPublisherMock.Verify(mock => mock.Gauge(expectedDefaultValue, metricName), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_InputDoesNotContainGenevaSettingsUsingGenevaFormat_ThrowsException()
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var metricValue = BogusGenerator.Random.Double();
            var metricFormat = StatsdFormatterTypesEnum.Geneva;
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var statsDPublisherMock = new Mock<IStatsDPublisher>();
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var statsDSinkConfiguration = CreateStatsDConfiguration(metricFormat);
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, metricsDeclarationProvider, statsDSinkConfiguration, NullLogger<StatsdMetricSink>.Instance);

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            await Assert.ThrowsAsync<ArgumentException>(() => metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult));
        }

        [Fact]
        public async Task ReportMetricAsync_UsesValidInputWithGenevaFormat_SuccessfullyWritesMetricWithRounding()
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var metricValue = BogusGenerator.Random.Double();
            var metricFormat = StatsdFormatterTypesEnum.Geneva;
            var genevaConfiguration = GenerateGenevaConfiguration();
            var measuredMetric = MeasuredMetric.CreateWithoutDimensions(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var statsDPublisherMock = new Mock<IStatsDPublisher>();
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var statsDSinkConfiguration = CreateStatsDConfiguration(metricFormat, genevaConfiguration);
            var metricSink = new StatsdMetricSink(statsDPublisherMock.Object, metricsDeclarationProvider, statsDSinkConfiguration, NullLogger<StatsdMetricSink>.Instance);
            var expectedMetricValue = Math.Round(metricValue, MidpointRounding.AwayFromZero);

            var bucket = JsonConvert.SerializeObject(new
            {
                statsDSinkConfiguration.CurrentValue.Geneva.Account,
                statsDSinkConfiguration.CurrentValue.Geneva.Namespace,
                Metric = metricName,
                Dims = metricSink.DetermineLabels(metricName, scrapeResult, measuredMetric)
            });

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            statsDPublisherMock.Verify(mock => mock.Gauge(expectedMetricValue, bucket), Times.Once());
        }

        private GenevaConfiguration GenerateGenevaConfiguration()
        {
            return new GenevaConfiguration { Account = BogusGenerator.Name.FirstName(), Namespace = BogusGenerator.Name.LastName() };
        }

        private IOptionsMonitor<StatsdSinkConfiguration> CreateStatsDConfiguration(
            StatsdFormatterTypesEnum formatterType = StatsdFormatterTypesEnum.Default,
            GenevaConfiguration geneva = null)
        {
            var statsDConfiguration = new StatsdSinkConfiguration
            {   
                MetricFormat = formatterType,
                Geneva = geneva
            };

            return new OptionsMonitorStub<StatsdSinkConfiguration>(statsDConfiguration);
        }
    }
}