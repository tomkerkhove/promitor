using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Promitor.Core.Metrics;
using Promitor.Integrations.Sinks.Atlassian.Statuspage;
using Promitor.Tests.Unit.Generators;
using Promitor.Tests.Unit.Generators.Config;
using Xunit;

namespace Promitor.Tests.Unit.Metrics.Sinks
{
    [Category("Unit")]
    public class AtlassianStatuspageMetricSinkTests
    {
        private readonly Faker _bogus = new Faker();

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ReportMetricAsync_InputDoesNotContainMetricName_ThrowsException(string metricName)
        {
            // Arrange
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var systemMetricConfigOptions = BogusAtlassianStatuspageMetricSinkConfigurationGenerator.GetSinkConfiguration();
            var atlassianStatuspageClientMock = new Mock<IAtlassianStatuspageClient>();
            var metricSink = new AtlassianStatuspageMetricSink(atlassianStatuspageClientMock.Object, systemMetricConfigOptions, NullLogger<AtlassianStatuspageMetricSink>.Instance);

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
            var metricName = _bogus.Name.FirstName();
            var metricValue = _bogus.Random.Double();
            var measuredMetric = MeasuredMetric.CreateWithoutDimension(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var systemMetricConfigOptions = BogusAtlassianStatuspageMetricSinkConfigurationGenerator.GetSinkConfiguration();
            var atlassianStatuspageClientMock = new Mock<IAtlassianStatuspageClient>();
            var metricSink = new AtlassianStatuspageMetricSink(atlassianStatuspageClientMock.Object, systemMetricConfigOptions, NullLogger<AtlassianStatuspageMetricSink>.Instance);

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);
        }

        [Fact]
        public async Task ReportMetricAsync_InputDoesNotContainMeasuredMetric_ThrowsException()
        {
            // Arrange
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var systemMetricConfigOptions = BogusAtlassianStatuspageMetricSinkConfigurationGenerator.GetSinkConfiguration();
            var atlassianStatuspageClientMock = new Mock<IAtlassianStatuspageClient>();
            var metricSink = new AtlassianStatuspageMetricSink(atlassianStatuspageClientMock.Object, systemMetricConfigOptions, NullLogger<AtlassianStatuspageMetricSink>.Instance);

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            await Assert.ThrowsAsync<ArgumentNullException>(() => metricSink.ReportMetricAsync(metricName, metricDescription, null));
        }

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithMetricValueAndPromitorToSystemMetricMapping_SuccessfullyWritesMetric()
        {
            // Arrange
            var promitorMetricName = _bogus.Name.FirstName();
            var systemMetricId = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
            var measuredMetric = MeasuredMetric.CreateWithoutDimension(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var systemMetricConfigOptions = BogusAtlassianStatuspageMetricSinkConfigurationGenerator.GetSinkConfiguration(systemMetricId: systemMetricId, promitorMetricName: promitorMetricName);
            var atlassianStatuspageClientMock = new Mock<IAtlassianStatuspageClient>();
            var metricSink = new AtlassianStatuspageMetricSink(atlassianStatuspageClientMock.Object, systemMetricConfigOptions, NullLogger<AtlassianStatuspageMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(promitorMetricName, metricDescription, scrapeResult);

            // Assert
            atlassianStatuspageClientMock.Verify(mock => mock.ReportMetricAsync(systemMetricId, metricValue), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithoutMetricValueButWithPromitorToSystemMetricMapping_SuccessfullyWritesMetricWithDefault()
        {
            // Arrange
            const double expectedDefaultValue = 0;
            var promitorMetricName = _bogus.Name.FirstName();
            var systemMetricId = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            double? metricValue = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var measuredMetric = MeasuredMetric.CreateWithoutDimension(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var systemMetricConfigOptions = BogusAtlassianStatuspageMetricSinkConfigurationGenerator.GetSinkConfiguration(systemMetricId: systemMetricId, promitorMetricName: promitorMetricName);
            var atlassianStatuspageClientMock = new Mock<IAtlassianStatuspageClient>();
            var metricSink = new AtlassianStatuspageMetricSink(atlassianStatuspageClientMock.Object, systemMetricConfigOptions, NullLogger<AtlassianStatuspageMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(promitorMetricName, metricDescription, scrapeResult);

            // Assert
            atlassianStatuspageClientMock.Verify(mock => mock.ReportMetricAsync(systemMetricId, expectedDefaultValue), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithPromitorMetricThatIsNotMappedToSystemMetricId_DoesNotWriteMetric()
        {
            // Arrange
            const double expectedDefaultValue = 0;
            var promitorMetricName = _bogus.Name.FirstName();
            var systemMetricId = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            double? metricValue = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var measuredMetric = MeasuredMetric.CreateWithoutDimension(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var systemMetricConfigOptions = BogusAtlassianStatuspageMetricSinkConfigurationGenerator.GetSinkConfiguration(promitorMetricName: promitorMetricName);
            var atlassianStatuspageClientMock = new Mock<IAtlassianStatuspageClient>();
            var metricSink = new AtlassianStatuspageMetricSink(atlassianStatuspageClientMock.Object, systemMetricConfigOptions, NullLogger<AtlassianStatuspageMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(promitorMetricName, metricDescription, scrapeResult);

            // Assert
            atlassianStatuspageClientMock.Verify(mock => mock.ReportMetricAsync(systemMetricId, expectedDefaultValue), Times.Never);
        }
    }
}