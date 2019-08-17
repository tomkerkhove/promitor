using System.Collections.Generic;
using Promitor.Core.Scraping.Configuration.Serialization.Enum;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class MetricsDeclarationV1
    {
        public string Version { get; set; } = SpecVersion.v1.ToString();

        public AzureMetadataV1 AzureMetadata { get; set; } = new AzureMetadataV1();
        
        public MetricDefaultsV1 MetricDefaults { get; set; } = new MetricDefaultsV1();

        public List<MetricDefinitionV1> Metrics { get; set; } = new List<MetricDefinitionV1>();
    }
}