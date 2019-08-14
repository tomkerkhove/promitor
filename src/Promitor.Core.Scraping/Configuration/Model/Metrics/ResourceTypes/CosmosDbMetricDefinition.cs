using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class CosmosDbMetricDefinition : MetricDefinition
    {
        public CosmosDbMetricDefinition()
        {
        }

        public CosmosDbMetricDefinition(AzureMetricConfiguration azureMetricConfiguration, string description, string name, string resourceGroupName, string dbName, Dictionary<string, string> labels, Scraping scraping)
            : base(name, description, resourceGroupName, labels, scraping, azureMetricConfiguration)
        {
            DbName = dbName;
        }

        public string DbName { get; set; }

        public override ResourceType ResourceType { get; } = ResourceType.CosmosDb;
    }
}
