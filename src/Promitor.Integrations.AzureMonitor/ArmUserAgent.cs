﻿using System;
using System.Collections.Generic;
using GuardNet;
using Promitor.Core;
using Promitor.Core.Metrics.Sinks;

namespace Promitor.Integrations.AzureMonitor
{
    public class ArmUserAgent
    {
        private const string EnabledText = "Enabled";
        private const string DisabledText = "Disabled";
        private const string ExtensionFormat = "{0} Prometheus/{1} OpenTelemetryCollector/{2} StatsD/{3} AtlassianStatuspage/{4}";

        public static string Generate(string agentVersion, List<MetricSinkType> metricSinkInfo)
        {
            Guard.NotNullOrWhitespace(agentVersion, nameof(agentVersion));
            Guard.NotNull(metricSinkInfo, nameof(metricSinkInfo));
            Guard.For<ArgumentException>(() => metricSinkInfo.Count == 0, nameof(metricSinkInfo));
            
            var originalUserAgent = UserAgent.Generate("Scraper", agentVersion);

            var prometheusState = GetRawState(MetricSinkType.PrometheusScrapingEndpoint, metricSinkInfo);
            var openTelemetryState =  GetRawState(MetricSinkType.OpenTelemetryCollector, metricSinkInfo);
            var statsDState =  GetRawState(MetricSinkType.StatsD, metricSinkInfo);
            var statuspageState =  GetRawState(MetricSinkType.AtlassianStatuspage, metricSinkInfo);

            return string.Format(ExtensionFormat, originalUserAgent, prometheusState, openTelemetryState, statsDState, statuspageState);
        }

        private static string GetRawState(MetricSinkType metricSinkType, List<MetricSinkType> enabledMetricSinks)
        {
            return enabledMetricSinks.Contains(metricSinkType)
                ? EnabledText
                : DisabledText;
        }
    }
}