using Promitor.Core.Configuration.Model.AzureMonitor;
using Promitor.Core.Configuration.Model.Metrics;
using Promitor.Core.Configuration.Model.Prometheus;
using Promitor.Core.Configuration.Model.Server;
using Promitor.Core.Configuration.Model.Telemetry;

namespace Promitor.Core.Configuration.Model
{
    public class RuntimeConfiguration
    {
        public ServerConfiguration Server { get; set; } = new ServerConfiguration();
        public PrometheusConfiguration Prometheus { get; set; } = new PrometheusConfiguration();
        public MetricsConfiguration MetricsConfiguration { get; set; } = new MetricsConfiguration();
        public TelemetryConfiguration Telemetry { get; set; } = new TelemetryConfiguration();
        public AzureMonitorConfiguration AzureMonitor { get; set; } = new AzureMonitorConfiguration();
    }
}