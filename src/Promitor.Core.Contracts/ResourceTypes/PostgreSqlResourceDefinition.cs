namespace Promitor.Core.Contracts.ResourceTypes
{
    public class PostgreSqlResourceDefinition : AzureResourceDefinition
    {
        public PostgreSqlResourceDefinition(string subscriptionId, string resourceGroupName, string serverName)
            : base(ResourceType.PostgreSql, subscriptionId, resourceGroupName, serverName)
        {
            ServerName = serverName;
        }

        public string ServerName { get; }
    }
}
