using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Scraper.Tests.Unit.Stubs
{
    public class MetricFake : IMetric
    {
        private List<TimeSeriesElement> _timeSeries;

        public MetricFake()
        {
            _timeSeries = new List<TimeSeriesElement>();
        }

        public void AddSeries(IEnumerable<MetricValue> series)
        {
            _timeSeries.Add(new TimeSeriesElement { Data = series.ToList() });
        }

        public string Id => throw new NotImplementedException();

        public ILocalizableString Name => throw new NotImplementedException();

        public IReadOnlyList<TimeSeriesElement> Timeseries => _timeSeries;

        public string Type => throw new NotImplementedException();

        public Microsoft.Azure.Management.Monitor.Fluent.Models.Unit Unit => throw new NotImplementedException();

        public Metric Inner => throw new NotImplementedException();
    }
}