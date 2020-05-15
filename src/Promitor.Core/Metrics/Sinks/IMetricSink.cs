using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promitor.Core.Metrics.Sinks
{
    public interface IMetricSink
    {
        MetricSinkType Type { get; }

        Task ReportMetricAsync(string metricName, string metricDescription, ScrapeResult scrapeResult);
        Task ReportMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels);
    }
}