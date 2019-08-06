using Microsoft.Extensions.Logging;

namespace Promitor.Core.Configuration
{
    public static class Defaults
    {
        public static class Server
        {
            public static int HttpPort { get; } = 80;
        }

        public static class Prometheus
        {
            public static string ScrapeEndpointBaseUri { get; } = "/metrics";
            public static double MetricUnavailableValue { get; } = double.NaN;
        }

        public static class MetricsConfiguration
        {
            public static string AbsolutePath { get; } = "/config/metrics-declaration.yaml";
        }
        
        public class Telemetry
        {
            public static LogLevel? DefaultVerbosity { get; set; } = LogLevel.Error;

            public class ContainerLogs
            {
                public static LogLevel? Verbosity { get; set; } = null;
                public static bool IsEnabled { get; set; } = true;
            }

            public class ApplicationInsights
            {
                public static LogLevel? Verbosity { get; set; } = null;
                public static bool IsEnabled { get; set; } = false;
            }
        }

        public static class FeatureFlags
        {
            public static bool DisableMetricTimestamps { get; set; } = false;
        }
    }
}
