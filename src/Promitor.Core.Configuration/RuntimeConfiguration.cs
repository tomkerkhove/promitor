using Promitor.Core.Configuration.Prometheus;
using Promitor.Core.Configuration.Server;
using Promitor.Core.Configuration.FeatureFlags;
using Promitor.Core.Configuration.Metrics;
using Promitor.Core.Configuration.Telemetry;

namespace Promitor.Core.Configuration
{
    public class RuntimeConfiguration
    {
        public ServerConfiguration Server { get; set; }
        public PrometheusConfiguration Prometheus { get; set; }
        public MetricsConfiguration MetricsConfiguration { get; set; }
        public virtual TelemetryConfiguration Telemetry { get; set; }
        public FeatureFlagsConfiguration FeatureFlags { get; set; }
    }
}