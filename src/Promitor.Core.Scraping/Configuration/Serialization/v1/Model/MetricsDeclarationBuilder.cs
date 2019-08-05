using System.Collections.Generic;
using System.Linq;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using YamlDotNet.Serialization;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class MetricsDeclarationBuilder
    {
        public string Version { get; set; }

        [YamlMember(Alias = "azureMetadata")]
        public AzureMetadataBuilder AzureMetadataBuilder { get; set; } = new AzureMetadataBuilder();

        [YamlMember(Alias = "metricDefaults")]
        public MetricDefaultsBuilder MetricDefaultsBuilder { get; set; } = new MetricDefaultsBuilder();

        public List<MetricDefinitionBuilder> Metrics { get; set; } = new List<MetricDefinitionBuilder>();

        public MetricsDeclaration Build()
        {
            return new MetricsDeclaration
            {
                Version = Version,
                AzureMetadata = AzureMetadataBuilder?.Build(),
                MetricDefaults = MetricDefaultsBuilder?.Build(),
                Metrics = Metrics.Select(builder => builder.Build()).ToList()
            };
        }
    }
}