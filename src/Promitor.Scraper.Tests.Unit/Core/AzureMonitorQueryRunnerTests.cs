using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Integrations.AzureMonitor;
using Promitor.Integrations.AzureMonitor.Exceptions;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Scraper.Host.Configuration.Serialization;
using Promitor.Scraper.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization
{
    [Category("Unit")]
    public class AzureMonitorQueryRunnerTests
    {
        [Fact]
        public void QueryRunnerTests_ExtractResult()
        {
            // Arrange
            var queryRunner = new AzureMonitorQueryRunner();

            List<MetricValue> series = new List<MetricValue>();
            series.Add(new MetricValue { Total = 1.0D, TimeStamp = new DateTime(2018, 10, 19, 12, 0, 0, DateTimeKind.Utc) });

            MetricFake metricFake = new MetricFake();
            metricFake.AddSeries(series);

            MetricCollectionFake collectionFake = new MetricCollectionFake();
            collectionFake.AddMetric(metricFake);

            // Act
            IEnumerable<MetricValue> resultSeries = queryRunner.ExtractResult(collectionFake, "metricName");

            // Assert
            Assert.Equal(series, resultSeries);            
        }

        [Fact]
        public void QueryRunnerTests_ExtractResult_EmptySeries()
        {
            // Arrange
            var queryRunner = new AzureMonitorQueryRunner();

            MetricFake metricFake = new MetricFake();
            metricFake.AddSeries(Enumerable.Empty<MetricValue>());

            MetricCollectionFake collectionFake = new MetricCollectionFake();
            collectionFake.AddMetric(metricFake);

            // Act
            IEnumerable<MetricValue> resultSeries = queryRunner.ExtractResult(collectionFake, "metricName");

            // Assert
            Assert.Empty(resultSeries);
        }

        [Fact]
        public void QueryRunnerTests_ExtractResult_TwoMetricsReturned()
        {
            // Arrange
            var queryRunner = new AzureMonitorQueryRunner();

            MetricCollectionFake collectionFake = new MetricCollectionFake();

            MetricFake metricFake = new MetricFake();
            metricFake.AddSeries(Enumerable.Empty<MetricValue>());
            collectionFake.AddMetric(metricFake);

            MetricFake anotherMetricFake = new MetricFake();
            metricFake.AddSeries(Enumerable.Empty<MetricValue>());
            collectionFake.AddMetric(anotherMetricFake);

            // Act

            // Assert
            Assert.Throws<MetricNotFoundException>(() => queryRunner.ExtractResult(collectionFake, "metricName"));
        }

        [Fact]
        public void QueryRunnerTests_ExtractResult_ZeroMetricsReturned()
        {
            // Arrange
            var queryRunner = new AzureMonitorQueryRunner();

            MetricCollectionFake collectionFake = new MetricCollectionFake();

            // Act

            // Assert
            Assert.Throws<MetricNotFoundException>(() => queryRunner.ExtractResult(collectionFake, "metricName"));
        }

        [Fact]
        public void QueryRunnerTests_ExtractResult_TwoTimeSeriesReturned()
        {
            // Arrange
            var queryRunner = new AzureMonitorQueryRunner();

            List<MetricValue> seriesOne = new List<MetricValue>();
            seriesOne.Add(new MetricValue { Total = 1.0D, TimeStamp = new DateTime(2018, 10, 19, 12, 0, 0, DateTimeKind.Utc) });
            List<MetricValue> seriesTwo = new List<MetricValue>();
            seriesTwo.Add(new MetricValue { Total = 2.0D, TimeStamp = new DateTime(2018, 10, 19, 12, 0, 0, DateTimeKind.Utc) });

            MetricFake metricFake = new MetricFake();
            metricFake.AddSeries(seriesOne);
            metricFake.AddSeries(seriesTwo);

            MetricCollectionFake collectionFake = new MetricCollectionFake();
            collectionFake.AddMetric(metricFake);

            // Act

            // Assert
            Assert.Throws<MetricInformationNotFoundException>(() => queryRunner.ExtractResult(collectionFake, "metricName"));
        }

        [Fact]
        public void QueryRunnerTests_ExtractResult_NoTimeSeriesReturned()
        {
            // Arrange
            var queryRunner = new AzureMonitorQueryRunner();

            MetricFake metricFake = new MetricFake();

            MetricCollectionFake collectionFake = new MetricCollectionFake();
            collectionFake.AddMetric(metricFake);

            // Act

            // Assert
            Assert.Throws<MetricInformationNotFoundException>(() => queryRunner.ExtractResult(collectionFake, "metricName"));
        }
    }
}