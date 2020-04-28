using Promitor.Core.Scraping;

namespace Promitor.Integrations.Sinks.Core
{
    public interface IMetricSink
    {
        void ReportMetric(string metricName, string metricDescription, ScrapeResult scrapedMetricResult);
    }
}