namespace Promitor.Core.Scraping.Sinks
{
    public interface IMetricSink
    {
        MetricSinkType SinkType { get; }

        void ReportMetric(string metricName, string metricDescription, ScrapeResult scrapedMetricResult);
    }
}