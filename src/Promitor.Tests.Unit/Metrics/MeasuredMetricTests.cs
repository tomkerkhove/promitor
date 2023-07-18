using System.Collections.Generic;
using System.ComponentModel;
using Promitor.Core.Metrics;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Xunit;
using Promitor.Core.Metrics.Exceptions;

namespace Promitor.Tests.Unit.Metrics
{
    [Category("Unit")]
    public class MeasuredMeticTest : UnitTest
    {
        [Fact]
        public void Create_MeasuredMetric_With_Dimension_HappyPath_Succeeds()
        {
            var dimensionName = "dimTest";
            var dimensionValue = "dimTest1";
            var timeSeries = new TimeSeriesElement(new List<MetadataValue> { new(name: new LocalizableString(dimensionName), value: dimensionValue)});
            var measuredMetric = MeasuredMetric.CreateForDimension(1, dimensionName, timeSeries);
            Assert.Equal(measuredMetric.DimensionName, dimensionName);
            Assert.Equal(measuredMetric.DimensionValue, dimensionValue);   
            Assert.Equal(measuredMetric.Value, 1);               
        }

        [Fact]
        public void Create_MeasuredMetric_Missing_Dimension_Throws_Targeted_Exception()
        {
            var dimensionName = "dimTest";
            var timeSeries = new TimeSeriesElement(new List<MetadataValue> {});
            MissingDimensionException ex = Assert.Throws<MissingDimensionException>(() => MeasuredMetric.CreateForDimension(1, dimensionName, timeSeries));
            Assert.Equal(ex.DimensionName, dimensionName);
        }
    }
}