namespace Promitor.Core.Contracts.ResourceTypes
{
    public class CosmosDbResourceDefinition : AzureResourceDefinition
    {
        public CosmosDbResourceDefinition(string subscriptionId, string resourceGroupName, string dbName)
            : base(ResourceType.CosmosDb, subscriptionId, resourceGroupName, dbName)
        {
            DbName = dbName;
        }

        public string DbName { get; }
    }
}
