using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class CosmosDbMetricDefinitionBuilder : MetricDefinitionBuilder
    {
        public string DbName { get; set; }

        public override ResourceType ResourceType { get; } = ResourceType.CosmosDb;

        public override MetricDefinition Build()
        {
            return new CosmosDbMetricDefinition(
                AzureMetricConfigurationBuilder.Build(),
                Description,
                Name,
                ResourceGroupName,
                DbName,
                Labels,
                ScrapingBuilder.Build());
        }
    }
}
