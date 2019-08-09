using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.Prometheus.Interfaces
{
    public interface IPrometheusMetricWriter
    {
        void ReportMetric(MetricDefinition metricDefinition, ScrapeResult scrapedMetricResult);
    }
}