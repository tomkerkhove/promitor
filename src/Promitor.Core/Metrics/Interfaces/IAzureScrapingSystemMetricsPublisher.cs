using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promitor.Core.Metrics.Interfaces
{
    public interface IAzureScrapingSystemMetricsPublisher : ISystemMetricsPublisher
    {
        /// <summary>
        ///     Sets a new value for a measurement on a gauge
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="description">Description of the metric</param>
        /// <param name="value">New measured value</param>
        /// <param name="labels">Labels that are applicable for this measurement</param>
        Task WriteGaugeMeasurementAsync(string name, string description, double value, Dictionary<string, string> labels);
    }
}