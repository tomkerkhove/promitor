using Promitor.Integrations.AzureMonitor;
using System;

namespace Promitor.Scraper.Host.Configuration.Model.Metrics.ResouceTypes
{
    public class GenericMetricDefinition : MetricDefinition
    {
        public MetricType MetricType { get; set; }
        public string Filter { get; set; }
        public override ResourceType ResourceType { get; set; } = ResourceType.Generic;
        public string ResourceUri { get; set; }
    }
}