using System.Threading.Tasks;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Core.Scraping.Sinks
{
    public interface IMetricSink
    {
        MetricSinkType Type { get; }

        Task ReportMetricAsync(string metricName, string metricDescription, MeasuredMetric measuredMetric);
    }
}