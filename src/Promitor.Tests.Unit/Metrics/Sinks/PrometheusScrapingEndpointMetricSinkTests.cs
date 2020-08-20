using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using Bogus;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Prometheus.Client.Abstractions;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Integrations.Sinks.Prometheus;
using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Promitor.Tests.Unit.Builders.Metrics.v1;
using Promitor.Tests.Unit.Generators;
using Promitor.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Tests.Unit.Metrics.Sinks
{
    [Category("Unit")]
    public class PrometheusScrapingEndpointMetricSinkTests
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
            var metricName = _bogus.Name.FirstName();
            var metricValue = _bogus.Random.Double();
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
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
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
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
            var scrapeResult = ScrapeResultGenerator.Generate(metricValue);
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

        [Fact]
        public async Task ReportMetricAsync_GetsValidInputWithTwoMetricValues_SuccessfullyWritesMultipleMetrics()
        {
            // Arrange
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var firstMetricValue = _bogus.Random.Double();
            var secondMetricValue = _bogus.Random.Double();
            var firstMetric = MeasuredMetric.CreateWithoutDimension(firstMetricValue);
            var secondMetric = MeasuredMetric.CreateWithoutDimension(secondMetricValue);
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
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var firstMetricValue = _bogus.Random.Double();
            var secondMetricValue = _bogus.Random.Double();
            var dimensionName = _bogus.Name.FirstName();
            var dimensionValue = _bogus.Name.FirstName();
            var expectedDimensionName = dimensionName.ToLower();
            var timeSeries = new TimeSeriesElement
            {
                Metadatavalues = new List<MetadataValue> { new MetadataValue(name: new LocalizableString(dimensionName), value: dimensionValue) }
            };
            var firstMetric = MeasuredMetric.CreateForDimension(firstMetricValue, dimensionName.ToUpper(), timeSeries);
            var secondMetric = MeasuredMetric.CreateWithoutDimension(secondMetricValue);
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
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
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
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
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
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(null)]
        public async Task ReportMetricAsync_GetsValidInputWithoutMetricValue_SuccessfullyWritesMetricWithDefault(double? expectedDefaultValue)
        {
            // Arrange
            double expectedMeasuredMetric = expectedDefaultValue??double.NaN; // If nothing is configured, NaN is used.
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            double? metricValue = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            var measuredMetric = MeasuredMetric.CreateWithoutDimension(metricValue);
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
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
            var dimensionName = _bogus.Name.FirstName();
            var dimensionValue = _bogus.Name.FirstName();
            var expectedDimensionName = dimensionName.ToLower();
            var timeSeries = new TimeSeriesElement
            {
                Metadatavalues = new List<MetadataValue> { new MetadataValue(name: new LocalizableString(dimensionName), value: dimensionValue) }
            };
            var measuredMetric = MeasuredMetric.CreateForDimension(metricValue, dimensionName.ToUpper(), timeSeries);
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
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
            var firstLabelName = _bogus.Name.FirstName();
            var secondLabelName = _bogus.Name.FirstName();
            var firstLabelValue = _bogus.Name.FirstName();
            var secondLabelValue = _bogus.Name.FirstName();
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
            var metricName = _bogus.Name.FirstName();
            var metricDescription = _bogus.Lorem.Sentence();
            var metricValue = _bogus.Random.Double();
            var labelName = _bogus.Name.FirstName();
            var scrapeResultLabelValue = _bogus.Name.FirstName();
            var metricConfigLabelValue = _bogus.Name.FirstName();
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

        private bool EnsureAllArrayEntriesAreSpecified(string[] specified, string[] expected)
        {
            if (specified.Length < expected.Length)
            {
                return false;
            }

            var outcome = Array.Exists(expected, entry=>specified.Contains(entry.ToLower()));
            return outcome;
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

        private MetricsDeclarationProviderStub CreateMetricsDeclarationProvider(string metricName,  Dictionary<string, string> labels = null)
        {
            var mapperConfiguration = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            var mapper = mapperConfiguration.CreateMapper();
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName, labels: labels)
                .Build(mapper);

            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, mapper);
            return metricsDeclarationProvider;
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