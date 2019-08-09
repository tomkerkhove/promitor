using System.Collections.Generic;
using System.Linq;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class MetricsDeclarationV1
    {
        public string Version { get; set; }
        
        public AzureMetadataV1 AzureMetadata { get; set; } = new AzureMetadataV1();
        
        public MetricDefaultsV1 MetricDefaults { get; set; } = new MetricDefaultsV1();

        public List<MetricDefinitionV1> Metrics { get; set; } = new List<MetricDefinitionV1>();

        public MetricsDeclaration Build()
        {
            return new MetricsDeclaration
            {
                Version = Version,
                AzureMetadata = AzureMetadata?.Build(),
                MetricDefaults = MetricDefaults?.Build(),
                Metrics = Metrics.Select(builder => builder.Build()).ToList()
            };
        }
    }
}