using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class CosmosDbMetricDefinitionV1 : MetricDefinitionV1
    {
        public string DbName { get; set; }

        public override ResourceType ResourceType { get; } = ResourceType.CosmosDb;

        public override MetricDefinition Build()
        {
            return new CosmosDbMetricDefinition(
                AzureMetricConfiguration.Build(),
                Description,
                Name,
                ResourceGroupName,
                DbName,
                Labels,
                Scraping.Build());
        }
    }
}
