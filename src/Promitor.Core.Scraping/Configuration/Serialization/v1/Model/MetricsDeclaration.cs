using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class MetricsDeclaration
    {
        public string Version { get; set; }
        public AzureMetadata AzureMetadata { get; set; }
        public MetricDefaults MetricDefaults { get; set; } = new MetricDefaults();
        public List<MetricDefinition> Metrics { get; set; } = new List<MetricDefinition>();
    }
}