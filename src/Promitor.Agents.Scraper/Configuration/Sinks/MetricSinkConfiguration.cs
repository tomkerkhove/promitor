﻿using Promitor.Integrations.Sinks.Prometheus.Configuration;
using Promitor.Integrations.Sinks.Statsd.Configuration;

namespace Promitor.Agents.Scraper.Configuration.Sinks
{
    public class MetricSinkConfiguration
    {
        public StatsdSinkConfiguration Statsd { get; set; }
        public PrometheusSinkConfiguration Prometheus { get; set; }
    }
}