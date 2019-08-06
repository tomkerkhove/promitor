namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class CosmosDbMetricDefinition : AzureResourceDefinition
    {
        public CosmosDbMetricDefinition() : base(ResourceType.CosmosDb)
        {
        }

        public CosmosDbMetricDefinition(string resourceGroupName, string dbName)
            : base(ResourceType.CosmosDb, resourceGroupName)
        {
            DbName = dbName;
        }

        public string DbName { get; set; }
    }
}
