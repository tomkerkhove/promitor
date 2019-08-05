using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class GenericAzureMetricDefinitionBuilder : MetricDefinitionBuilder
    {
        public string Filter { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.Generic;
        public string ResourceUri { get; set; }

        public override MetricDefinition Build()
        {
            return new GenericAzureMetricDefinition(
                AzureMetricConfigurationBuilder.Build(),
                Description,
                Name,
                ResourceGroupName,
                Filter,
                ResourceUri,
                Labels,
                ScrapingBuilder.Build());
        }
    }
}