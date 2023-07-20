using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Prometheus.Client;
using Promitor.Core.Metrics;
using Promitor.Integrations.Sinks.Prometheus;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Promitor.Tests.Unit.Generators;
using Promitor.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Tests.Unit.Metrics.Sinks
{
    [Category("Unit")]
    public class PrometheusScrapingEndpointMetricSinkTest : MetricSinkTest
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
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var prometheusConfiguration = CreatePrometheusConfiguration();
            var metricFactoryMock = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(metricFactoryMock.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);

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
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var prometheusConfiguration = CreatePrometheusConfiguration();
            var metricFactoryMock = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(metricFactoryMock.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);

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
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var prometheusConfiguration = CreatePrometheusConfiguration();
            var metricFactoryMock = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(metricFactoryMock.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);

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
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var defaultLabels = new Dictionary<string, string>
            {
                {"app", "promitor"}
            };
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName, defaultLabels: defaultLabels);
            var prometheusConfiguration = CreatePrometheusConfiguration();
            var mocks = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(mocks.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            mocks.Factory.Verify(mock => mock.CreateGauge(metricName, metricDescription, It.IsAny<bool>(), It.Is<string[]>(specified => EnsureAllLabelKeysAreSpecified(specified, scrapeResult.Labels.Keys.ToArray(), defaultLabels.Keys.ToArray()))), Times.Once());
            mocks.MetricFamily.Verify(mock => mock.WithLabels(It.Is<string[]>(specified => EnsureAllLabelValuesAreSpecified(specified, scrapeResult.Labels.Values.ToArray(), defaultLabels.Values.ToArray()))), Times.Once());
            mocks.Gauge.Verify(mock => mock.Set(metricValue), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithTwoMetricValues_SuccessfullyWritesMultipleMetrics()
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var firstMetricValue = BogusGenerator.Random.Double();
            var secondMetricValue = BogusGenerator.Random.Double();
            var firstMetric = MeasuredMetric.CreateWithoutDimensions(firstMetricValue);
            var secondMetric = MeasuredMetric.CreateWithoutDimensions(secondMetricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(firstMetric);
            scrapeResult.MetricValues.Add(secondMetric);
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var prometheusConfiguration = CreatePrometheusConfiguration();
            var mocks = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(mocks.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            mocks.Factory.Verify(mock => mock.CreateGauge(metricName, metricDescription, It.IsAny<bool>(), It.Is<string[]>(specified => EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Keys.ToArray()))), Times.Exactly(2));
            mocks.MetricFamily.Verify(mock => mock.WithLabels(It.Is<string[]>(specified => EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Values.ToArray()))), Times.Exactly(2));
            mocks.Gauge.Verify(mock => mock.Set(firstMetricValue), Times.Once());
            mocks.Gauge.Verify(mock => mock.Set(secondMetricValue), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithTwoMetricValuesOfWhichOneIsMultiDimensional_SuccessfullyWritesMultipleMetrics()
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var firstMetricValue = BogusGenerator.Random.Double();
            var secondMetricValue = BogusGenerator.Random.Double();
            var dimensionName = BogusGenerator.Name.FirstName();
            var dimensionValue = BogusGenerator.Name.FirstName();
            var expectedDimensionName = dimensionName.ToLower();
            var timeSeries = new TimeSeriesElement
            {
                Metadatavalues = new List<MetadataValue> { new(name: new LocalizableString(dimensionName), value: dimensionValue) }
            };
            var firstMetric = MeasuredMetric.CreateForDimensions(firstMetricValue, new List<string>{ dimensionName.ToUpper() }, timeSeries);
            var secondMetric = MeasuredMetric.CreateWithoutDimensions(secondMetricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(firstMetric);
            scrapeResult.MetricValues.Add(secondMetric);
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var prometheusConfiguration = CreatePrometheusConfiguration();
            var mocks = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(mocks.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            mocks.Factory.Verify(mock => mock.CreateGauge(metricName, metricDescription, It.IsAny<bool>(), It.Is<string[]>(specified => specified.Contains(expectedDimensionName) && EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Keys.ToArray()))), Times.Once());
            mocks.Factory.Verify(mock => mock.CreateGauge(metricName, metricDescription, It.IsAny<bool>(), It.Is<string[]>(specified => specified.Contains(expectedDimensionName) == false && EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Keys.ToArray()))), Times.Once);
            mocks.MetricFamily.Verify(mock => mock.WithLabels(It.Is<string[]>(specified => specified.Contains(dimensionValue) && EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Values.ToArray()))), Times.Once);
            mocks.MetricFamily.Verify(mock => mock.WithLabels(It.Is<string[]>(specified => specified.Contains(dimensionValue) == false && EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Values.ToArray()))), Times.Once);
            mocks.Gauge.Verify(mock => mock.Set(firstMetricValue), Times.Once());
            mocks.Gauge.Verify(mock => mock.Set(secondMetricValue), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithUpperCaseLabelNames_SuccessfullyWritesMetricWithLowercaseLabels()
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var metricValue = BogusGenerator.Random.Double();
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var mutatedLabels = scrapeResult.Labels.ToDictionary(kvp => kvp.Key.ToUpper(), kvp => kvp.Value);
            scrapeResult.Labels.Clear();
            mutatedLabels.ForAll(kvp=> scrapeResult.Labels.Add(kvp.Key.ToUpper(),kvp.Value));
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var prometheusConfiguration = CreatePrometheusConfiguration();
            var mocks = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(mocks.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            mocks.Factory.Verify(mock => mock.CreateGauge(metricName, metricDescription, It.IsAny<bool>(), It.Is<string[]>(specified => EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Keys.ToArray()))), Times.Once());
            mocks.MetricFamily.Verify(mock => mock.WithLabels(It.Is<string[]>(specified => EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Values.ToArray()))), Times.Once());
            mocks.Gauge.Verify(mock => mock.Set(metricValue), Times.Once());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ReportMetricAsync_GetsValidInputWithMetricValueAndTimestampFlag_SuccessfullyWritesMetricWithRespectToTimestampFlag(bool includeTimestampsInMetrics)
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var metricValue = BogusGenerator.Random.Double();
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var prometheusConfiguration = CreatePrometheusConfiguration(enableMetricsTimestamps: includeTimestampsInMetrics);
            var mocks = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(mocks.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            mocks.Factory.Verify(mock => mock.CreateGauge(metricName, metricDescription, includeTimestampsInMetrics, It.Is<string[]>(specified => EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Keys.ToArray()))), Times.Once());
            mocks.MetricFamily.Verify(mock => mock.WithLabels(It.Is<string[]>(specified => EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Values.ToArray()))), Times.Once());
            mocks.Gauge.Verify(mock => mock.Set(metricValue), Times.Once());
        }

        [Theory]
        [InlineData((double)0)]
        [InlineData((double)-1)]
        [InlineData(null)]
        public async Task ReportMetricAsync_GetsValidInputWithoutMetricValue_SuccessfullyWritesMetricWithDefault(double? expectedDefaultValue)
        {
            // Arrange
            double expectedMeasuredMetric = expectedDefaultValue??double.NaN; // If nothing is configured, NaN is used.
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            double? metricValue = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var measuredMetric = MeasuredMetric.CreateWithoutDimensions(metricValue);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var prometheusConfiguration = CreatePrometheusConfiguration(metricUnavailableValue: expectedDefaultValue);
            var mocks = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(mocks.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            mocks.Factory.Verify(mock => mock.CreateGauge(metricName, metricDescription, It.IsAny<bool>(), It.Is<string[]>(specified => EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Keys.ToArray()))), Times.Once());
            mocks.MetricFamily.Verify(mock => mock.WithLabels(It.Is<string[]>(specified => EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Values.ToArray()))), Times.Once());
            mocks.Gauge.Verify(mock => mock.Set(expectedMeasuredMetric), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithOneDimensions_SuccessfullyWritesMetric()
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var metricValue = BogusGenerator.Random.Double();
            var dimensionName = BogusGenerator.Name.FirstName();
            var dimensionValue = BogusGenerator.Name.FirstName();
            var expectedDimensionName = dimensionName.ToLower();
            var timeSeries = new TimeSeriesElement
            {
                Metadatavalues = new List<MetadataValue> { new(name: new LocalizableString(dimensionName), value: dimensionValue) }
            };
            var measuredMetric = MeasuredMetric.CreateForDimensions(metricValue, new List<string>{ dimensionName.ToUpper() }, timeSeries);
            var scrapeResult = ScrapeResultGenerator.GenerateFromMetric(measuredMetric);
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName);
            var prometheusConfiguration = CreatePrometheusConfiguration();
            var mocks = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(mocks.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            mocks.Factory.Verify(mock => mock.CreateGauge(metricName, metricDescription, It.IsAny<bool>(), It.Is<string[]>(specified => specified.Contains(expectedDimensionName) && EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Keys.ToArray()))), Times.Once());
            mocks.MetricFamily.Verify(mock => mock.WithLabels(It.Is<string[]>(specified => specified.Contains(dimensionValue) && EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Values.ToArray()))), Times.Once());
            mocks.Gauge.Verify(mock => mock.Set(metricValue), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithDefaultMetricLabelsConfigured_SuccessfullyWritesMetric()
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var metricValue = BogusGenerator.Random.Double();
            var firstLabelName = BogusGenerator.Name.FirstName();
            var secondLabelName = BogusGenerator.Name.FirstName();
            var firstLabelValue = BogusGenerator.Name.FirstName();
            var secondLabelValue = BogusGenerator.Name.FirstName();
            var expectedFirstLabelName = firstLabelName.ToLower();
            var expectedSecondLabelName = secondLabelName.ToLower();
            var configuredLabels = new Dictionary<string, string>
            {
                {firstLabelName.ToUpper(), firstLabelValue},
                {secondLabelName.ToUpper(), secondLabelValue}
            };
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName, configuredLabels);
            var prometheusConfiguration = CreatePrometheusConfiguration();
            var mocks = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(mocks.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);

            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            mocks.Factory.Verify(mock => mock.CreateGauge(metricName, metricDescription, It.IsAny<bool>(), It.Is<string[]>(specified => specified.Contains(expectedFirstLabelName) && specified.Contains(expectedSecondLabelName) && EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Keys.ToArray()))), Times.Once());
            mocks.MetricFamily.Verify(mock => mock.WithLabels(It.Is<string[]>(specified => specified.Contains(firstLabelValue) && specified.Contains(secondLabelValue) && EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Values.ToArray()))), Times.Once());
            mocks.Gauge.Verify(mock => mock.Set(metricValue), Times.Once());
        }

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithMetricLabelWithSameKeyAsScrapeResult_SuccessfullyWritesMetricWithLabelValueFromScrapeResult()
        {
            // Arrange
            var metricName = BogusGenerator.Name.FirstName();
            var metricDescription = BogusGenerator.Lorem.Sentence();
            var metricValue = BogusGenerator.Random.Double();
            var labelName = BogusGenerator.Name.FirstName();
            var scrapeResultLabelValue = BogusGenerator.Name.FirstName();
            var metricConfigLabelValue = BogusGenerator.Name.FirstName();
            var expectedLabelName = labelName.ToLower();
            var configuredLabels = new Dictionary<string, string>
            {
                {labelName.ToUpper(), metricConfigLabelValue}
            };
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
            scrapeResult.Labels.Add(labelName, scrapeResultLabelValue);
            var metricsDeclarationProvider = CreateMetricsDeclarationProvider(metricName, configuredLabels);
            var prometheusConfiguration = CreatePrometheusConfiguration();
            var mocks = CreatePrometheusMetricFactoryMock();
            var metricSink = new PrometheusScrapingEndpointMetricSink(mocks.Factory.Object, metricsDeclarationProvider, prometheusConfiguration, NullLogger<PrometheusScrapingEndpointMetricSink>.Instance);
            
            // Act
            await metricSink.ReportMetricAsync(metricName, metricDescription, scrapeResult);

            // Assert
            mocks.Factory.Verify(mock => mock.CreateGauge(metricName, metricDescription, It.IsAny<bool>(), It.Is<string[]>(specified => specified.Contains(expectedLabelName) && EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Keys.ToArray()))), Times.Once());
            mocks.MetricFamily.Verify(mock => mock.WithLabels(It.Is<string[]>(specified => specified.Contains(scrapeResultLabelValue) && specified.Contains(metricConfigLabelValue) == false && EnsureAllArrayEntriesAreSpecified(specified, scrapeResult.Labels.Values.ToArray()))), Times.Once());
            mocks.Gauge.Verify(mock => mock.Set(metricValue), Times.Once());
        }

        private bool EnsureAllArrayEntriesAreSpecified(string[] specified, string[] expectedMetricLabels)
        {
            if (specified.Length < expectedMetricLabels.Length)
            {
                return false;
            }

            var outcome = Array.Exists(expectedMetricLabels, entry => specified.Contains(entry.ToLower()));
            return outcome;
        }
        
        private bool EnsureAllLabelValuesAreSpecified(string[] specified, string[] expectedMetricLabels, string[] expectedDefaultLabels)
        {
            return EnsureAllArrayEntriesAreSpecified(specified, expectedMetricLabels, expectedDefaultLabels, "tenantId");
        }

        private bool EnsureAllLabelKeysAreSpecified(string[] specified, string[] expectedMetricLabels, string[] expectedDefaultLabels)
        {
            return EnsureAllArrayEntriesAreSpecified(specified, expectedMetricLabels, expectedDefaultLabels, "tenant_id");
        }

        private bool EnsureAllArrayEntriesAreSpecified(string[] specified, string[] expectedMetricLabels, string[] expectedDefaultLabels, string expectedTenantId)
        {
            // We are adding 1 for the tenant id label
            var expectedTotalLabelCount = expectedDefaultLabels.Length + expectedMetricLabels.Length + 1;
            if (specified.Length != expectedTotalLabelCount)
            {
                return false;
            }

            var isSuccessful = Array.Exists(expectedMetricLabels, entry => specified.Contains(entry.ToLower()));
            if(isSuccessful)
            {
                isSuccessful = Array.Exists(expectedMetricLabels, entry => specified.Contains(entry.ToLower()));
            }

            if(isSuccessful)
            {
                isSuccessful = specified.Contains(expectedTenantId);
            }

            return isSuccessful;
        }

        private IOptionsMonitor<PrometheusScrapingEndpointSinkConfiguration> CreatePrometheusConfiguration(bool enableMetricsTimestamps = true, double? metricUnavailableValue = -1)
        {
            var prometheusScrapingEndpointSinkConfiguration = new PrometheusScrapingEndpointSinkConfiguration
            {
                BaseUriPath = "/test-metrics",
                EnableMetricTimestamps = enableMetricsTimestamps,
                MetricUnavailableValue = metricUnavailableValue
            };

            return new OptionsMonitorStub<PrometheusScrapingEndpointSinkConfiguration>(prometheusScrapingEndpointSinkConfiguration);
        }

        private static (Mock<IMetricFactory> Factory, Mock<IMetricFamily<IGauge>> MetricFamily, Mock<IGauge> Gauge) CreatePrometheusMetricFactoryMock()
        {
            var gaugeMock = new Mock<IGauge>();
            var gaugeMetricFamilyMock = new Mock<IMetricFamily<IGauge>>();
            gaugeMetricFamilyMock.Setup(gauge => gauge.WithLabels(It.IsAny<string[]>()))
                .Returns(gaugeMock.Object);

            var metricFactoryMock = new Mock<IMetricFactory>();
            metricFactoryMock.Setup(factory => factory.CreateGauge(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string[]>()))
                             .Returns(gaugeMetricFamilyMock.Object);

            return (metricFactoryMock, gaugeMetricFamilyMock, gaugeMock);
        }
    }
}
