namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class CosmosDbResourceDefinition : AzureResourceDefinition
    {
        public CosmosDbResourceDefinition(string subscriptionId, string resourceGroupName, string dbName)
            : base(ResourceType.CosmosDb, subscriptionId, resourceGroupName)
        {
            DbName = dbName;
        }

        public string DbName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => DbName;
    }
}
