using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class CosmosDbMetricDefinitionV1 : MetricDefinitionV1
    {
        public string DbName { get; set; }

        public override ResourceType ResourceType { get; } = ResourceType.CosmosDb;
    }
}
