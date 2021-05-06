﻿using Promitor.Integrations.Sinks.Prometheus.Configuration;

namespace Promitor.Integrations.Sinks.Prometheus
{
    public static class Defaults
    {
        public static class Prometheus
        {
            public static bool EnableMetricTimestamps { get; set; } = false;
            public static double MetricUnavailableValue { get; } = double.NaN;
            public static string ScrapeEndpointBaseUri { get; } = "/metrics";
            public static LabelTransformation LabelTransformation { get; } = LabelTransformation.None;
        }
    }
}
