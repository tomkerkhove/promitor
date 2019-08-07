using System.Collections;
using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model
{
    /// <summary>
    /// Represents the metrics configuration file.
    /// </summary>
    public class MetricsDeclarationV2
    {
        public string Version { get; set; }
        public AzureMetadataV2 AzureMetadata { get; set; }
        public MetricDefaultsV2 MetricDefaults { get; set; }
        public List<MetricDefinitionV2> Metrics { get; set; }
    }
}
