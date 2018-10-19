using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bogus;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Integrations.AzureMonitor;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Model.Metrics.ResouceTypes;
using Promitor.Scraper.Host.Configuration.Serialization;
using Promitor.Scraper.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization
{
    [Category("Unit")]
    public class DataPointReaderTests
    {
        [Fact]
        public void DataPointReaderTests_QueryOptions_TransactionalMetric()
        {
            // Arrange
            IEnumerable<MetricValue> queryResult = Enumerable.Empty<MetricValue>();
            var queryRunnerFake = new QueryRunnerFake(queryResult);
            var readDataNew = new DataPointReader(queryRunnerFake);

            DateTime utcNow = DateTime.UtcNow;
            DateTime trimmedUtcNow = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second, DateTimeKind.Utc);
            DateTime twentyMinsPrior = trimmedUtcNow.AddMinutes(-20.0D);

            // Act
            var queryOptions = readDataNew.CreateOptions(MetricType.Transactional, AggregationType.Total, "dimension eq 'Alternate'", utcNow);

            // Assert
            Assert.Equal(AggregationType.Total, queryOptions.AggregationType);
            Assert.Equal(trimmedUtcNow, queryOptions.EndTime);
            Assert.Equal(twentyMinsPrior, queryOptions.StartTime);
            Assert.Equal(TimeSpan.FromMinutes(1), queryOptions.MetricGranularity);
            Assert.Equal("dimension eq 'Alternate'", queryOptions.MetricFilter);
        }

        [Fact]
        public void DataPointReaderTests_QueryOptions_CapacityMetric()
        {
            // Arrange
            IEnumerable<MetricValue> queryResult = Enumerable.Empty<MetricValue>();
            var queryRunnerFake = new QueryRunnerFake(queryResult);
            var readDataNew = new DataPointReader(queryRunnerFake);

            DateTime utcNow = DateTime.UtcNow;
            DateTime trimmedUtcNow = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second, DateTimeKind.Utc);
            DateTime twoHoursPrior = trimmedUtcNow.AddHours(-2.0D);

            // Act
            var queryOptions = readDataNew.CreateOptions(MetricType.Capacity, AggregationType.Count, null, utcNow);

            // Assert
            Assert.Equal(AggregationType.Count, queryOptions.AggregationType);
            Assert.Equal(trimmedUtcNow, queryOptions.EndTime);
            Assert.Equal(twoHoursPrior, queryOptions.StartTime);
            Assert.Equal(TimeSpan.FromHours(1), queryOptions.MetricGranularity);
            Assert.Null(queryOptions.MetricFilter);
        }

        [Fact]
        public void DataPointReaderTests_ExtractPoint_MostRecent()
        {
            // Arrange
            IEnumerable<MetricValue> queryResult = Enumerable.Empty<MetricValue>();
            var queryRunnerFake = new QueryRunnerFake(queryResult);
            var readDataNew = new DataPointReader(queryRunnerFake);

            DateTime utcNow = DateTime.UtcNow;
            DateTime trimmedUtcNow = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, 0, DateTimeKind.Utc);

            var metricValueList = new List<MetricValue>();
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddMinutes(-0D), Total = 1.0D }); // Most recent
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddMinutes(-1D), Total = 2.0D });
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddMinutes(-2D), Total = 3.0D });
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddMinutes(-3D), Total = 4.0D });

            // Act
            double point = readDataNew.ExtractDataPoint(metricValueList, trimmedUtcNow, AggregationType.Total);

            // Assert
            Assert.Equal(1.0D, point);
        }

        [Fact]
        public void DataPointReaderTests_ExtractPoint_MostRecentIsNull()
        {
            // Arrange
            IEnumerable<MetricValue> queryResult = Enumerable.Empty<MetricValue>();
            var queryRunnerFake = new QueryRunnerFake(queryResult);
            var readDataNew = new DataPointReader(queryRunnerFake);

            DateTime utcNow = DateTime.UtcNow;
            DateTime trimmedUtcNow = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, 0, DateTimeKind.Utc);

            var metricValueList = new List<MetricValue>();
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddMinutes(-0D), Total = null }); // Most recent
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddMinutes(-1D), Total = 2.0D });
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddMinutes(-2D), Total = 3.0D });
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddMinutes(-3D), Total = 4.0D });

            // Act
            double point = readDataNew.ExtractDataPoint(metricValueList, trimmedUtcNow, AggregationType.Total);

            // Assert
            Assert.Equal(DataPointReader.NO_DATA, point);
        }

        [Fact]
        public void DataPointReaderTests_ExtractPoint_MostRecentEnddateCutoff()
        {
            // Arrange
            IEnumerable<MetricValue> queryResult = Enumerable.Empty<MetricValue>();
            var queryRunnerFake = new QueryRunnerFake(queryResult);
            var readDataNew = new DataPointReader(queryRunnerFake);

            DateTime utcNow = DateTime.UtcNow;
            DateTime trimmedUtcNow = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, 0, DateTimeKind.Utc);

            var metricValueList = new List<MetricValue>();
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddSeconds(+1D), Total = 100D }); // Most recent but past cutoff of enddate
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddMinutes(-0D), Total = null });
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddMinutes(-1D), Total = 3.0D });
            metricValueList.Add(new MetricValue { TimeStamp = trimmedUtcNow.AddMinutes(-2D), Total = 4.0D });

            // Act
            double point = readDataNew.ExtractDataPoint(metricValueList, trimmedUtcNow, AggregationType.Total);

            // Assert
            Assert.Equal(DataPointReader.NO_DATA, point); // Default
        }
    }
}