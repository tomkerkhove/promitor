namespace Promitor.Core.Metrics.Sinks
{
    public enum MetricSinkType
    {
        PrometheusScrapingEndpoint,
        StatsD,
        AtlassianStatuspage,
        OpenTelemetryCollector
    }
}