using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promitor.Core.Metrics.Interfaces
{
    public interface ISystemMetricsPublisher
    {
        /// <summary>
        ///     Sets a new value for a measurement on a gauge
        /// </summary>
        /// <param name="name">Name of the metric</param>
        /// <param name="description">Description of the metric</param>
        /// <param name="value">New measured value</param>
        /// <param name="labels">Labels that are applicable for this measurement</param>
        /// <param name="includeTimestamp">Indication whether or not a timestamp should be reported</param>
        Task WriteGaugeMeasurementAsync(string name, string description, double value, Dictionary<string, string> labels, bool includeTimestamp);
    }
}
