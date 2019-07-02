using Promitor.Core.Configuration.Prometheus;
using Promitor.Core.Configuration.Server;
using Promitor.Core.Configuration.FeatureFlags;
using Promitor.Core.Configuration.Metrics;
using Promitor.Core.Configuration.Telemetry;

namespace Promitor.Core.Configuration
{
    public class RuntimeConfiguration
    {
        public ServerConfiguration Server { get; set; } = new ServerConfiguration();
        public PrometheusConfiguration Prometheus { get; set; } = new PrometheusConfiguration();
        public MetricsConfiguration MetricsConfiguration { get; set; } = new MetricsConfiguration();
        public TelemetryConfiguration Telemetry { get; set; } = new TelemetryConfiguration();
        public FeatureFlagsConfiguration FeatureFlags { get; set; } = new FeatureFlagsConfiguration();
    }
}