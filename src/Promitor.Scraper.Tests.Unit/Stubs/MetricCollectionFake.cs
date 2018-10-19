using System;
using System.Collections.Generic;
using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Scraper.Tests.Unit.Stubs
{

    public class MetricCollectionFake : IMetricCollection
    {
        private List<IMetric> _list;

        public MetricCollectionFake()
        {
            _list = new List<IMetric>();
        }

        public void AddMetric(MetricFake metricFake)
        {
            _list.Add(metricFake);
        }

        public double? Cost => 0.0D;

        public TimeSpan? Interval => TimeSpan.FromMinutes(1);

        public IReadOnlyList<IMetric> Metrics => _list;

        public string Namespace => "dummy";

        public string ResourceRegion => "dummyregion";

        public string Timespan => "";

        public ResponseInner Inner => throw new NotImplementedException();
    }
}