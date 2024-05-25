using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Azure.Monitor.Query.Models;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Metrics;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Xunit;

namespace Promitor.Tests.Unit.Core.Scraping.Configuration.Model.Metrics
{
    [Category("Unit")]
    public class MetricAggregationTypeConverterTest
    {
        private readonly Dictionary<PromitorMetricAggregationType, AggregationType> _expectedAggregationTypeMappingLegacy = new Dictionary<PromitorMetricAggregationType, AggregationType>
        {
            { PromitorMetricAggregationType.Average, AggregationType.Average },
            { PromitorMetricAggregationType.Count, AggregationType.Count },
            { PromitorMetricAggregationType.Maximum, AggregationType.Maximum },
            { PromitorMetricAggregationType.Minimum, AggregationType.Minimum },
            { PromitorMetricAggregationType.None, AggregationType.None },
            { PromitorMetricAggregationType.Total, AggregationType.Total },
        };

        private readonly Dictionary<PromitorMetricAggregationType, MetricAggregationType> _expectedAggregationTypeMapping = new Dictionary<PromitorMetricAggregationType, MetricAggregationType>
        {
            { PromitorMetricAggregationType.Average, MetricAggregationType.Average },
            { PromitorMetricAggregationType.Maximum, MetricAggregationType.Maximum },
            { PromitorMetricAggregationType.Minimum, MetricAggregationType.Minimum },
            { PromitorMetricAggregationType.None, MetricAggregationType.None },
            { PromitorMetricAggregationType.Total, MetricAggregationType.Total },
        };



        [Fact]
        public void ConvertAggregationType_Legacy()
        {
            var promitorAggregationTypes = (PromitorMetricAggregationType[])Enum.GetValues(typeof(PromitorMetricAggregationType));
            foreach (var promitorAggregationType in promitorAggregationTypes) 
            {
                Assert.Equal(_expectedAggregationTypeMappingLegacy[promitorAggregationType], MetricAggregationTypeConverter.AsLegacyAggregationType(promitorAggregationType));
            }
        }

        [Fact]
        public void ConvertAggregationType()
        {
            var promitorAggregationTypes = (PromitorMetricAggregationType[])Enum.GetValues(typeof(PromitorMetricAggregationType));
            foreach (var promitorAggregationType in promitorAggregationTypes) 
            {
                Assert.Equal(_expectedAggregationTypeMapping[promitorAggregationType], MetricAggregationTypeConverter.AsMetricAggregationType(promitorAggregationType));
            }
        }
    }
}
