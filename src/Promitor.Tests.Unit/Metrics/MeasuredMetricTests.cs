using System.Collections.Generic;
using System.ComponentModel;
using Promitor.Core.Metrics;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Xunit;
using Promitor.Core.Metrics.Exceptions;

namespace Promitor.Tests.Unit.Metrics
{
    [Category("Unit")]
    public class MeasuredMetricTest : UnitTest
    {
        [Fact]
        public void Create_MeasuredMetric_With_Single_Dimension_HappyPath_Succeeds()
        {
            var dimensionNames = new List<string> { "dimTest"};
            var dimensionValue = "dimTest1";
            var timeSeries = new TimeSeriesElement(new List<MetadataValue> { new(name: new LocalizableString(dimensionNames[0]), value: dimensionValue)});
            var measuredMetric = MeasuredMetric.CreateForDimensions(1, dimensionNames, timeSeries);
            Assert.Equal(measuredMetric.Dimensions.Count, 1);
            Assert.Equal(measuredMetric.Dimensions[0].Name, dimensionNames[0]);
            Assert.Equal(measuredMetric.Dimensions[0].Value, dimensionValue);   
            Assert.Equal(measuredMetric.Value, 1);               
        }

        [Fact]
        public void Create_MeasuredMetric_Missing_Dimension_Throws_Targeted_Exception()
        {
            var dimensionName = new List<string> { "dimTest"};
            var timeSeries = new TimeSeriesElement(new List<MetadataValue> {});
            MissingDimensionException ex = Assert.Throws<MissingDimensionException>(() => MeasuredMetric.CreateForDimensions(1, dimensionName, timeSeries));
            Assert.Equal(ex.DimensionName, dimensionName[0]);
        }
    }
}