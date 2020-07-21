using System.Collections.Generic;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    /// <summary>
    /// Represents the metrics configuration file.
    /// </summary>
    public class MetricsDeclarationV1
    {
        public string Version { get; set; } = SpecVersion.v1.ToString();
        public AzureMetadataV1 AzureMetadata { get; set; }
        public MetricDefaultsV1 MetricDefaults { get; set; }
        public IReadOnlyCollection<MetricDefinitionV1> Metrics { get; set; }
    }
}
