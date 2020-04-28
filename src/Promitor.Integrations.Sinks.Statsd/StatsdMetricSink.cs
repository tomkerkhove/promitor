using System;
using Promitor.Core.Scraping;
using Promitor.Integrations.Sinks.Core;

namespace Promitor.Integrations.Sinks.Statsd
{
    public class StatsdMetricSink : IMetricSink
    {
        public void ReportMetric(string metricName, string metricDescription, ScrapeResult scrapedMetricResult)
        {
            throw new NotImplementedException();
        }
    }
}
