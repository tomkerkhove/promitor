using Promitor.Integrations.AzureMonitor;

namespace Promitor.Integrations.Sinks.Core
{
    public interface IMetricSink
    {
        MetricSinkType SinkType { get; }

        void ReportMetric(string metricName, string metricDescription, MeasuredMetric measuredMetric);
    }
}