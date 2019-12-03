using Microsoft.Extensions.Logging;
using Serilog.Events;

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
            public static bool EnableMetricTimestamps { get; set; } = false;
            public static double MetricUnavailableValue { get; } = double.NaN;
            public static string ScrapeEndpointBaseUri { get; } = "/metrics";
        }

        public static class MetricsConfiguration
        {
            public static string AbsolutePath { get; } = "/config/metrics-declaration.yaml";
        }
        
        public class Telemetry
        {
            public static LogEventLevel? DefaultVerbosity { get; set; } = LogEventLevel.Error;

            public class ContainerLogs
            {
                public static LogEventLevel? Verbosity { get; set; } = null;
                public static bool IsEnabled { get; set; } = true;
            }

            public class ApplicationInsights
            {
                public static LogEventLevel? Verbosity { get; set; } = null;
                public static bool IsEnabled { get; set; } = false;
            }
        }
    }
}
