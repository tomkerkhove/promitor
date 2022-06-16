using Promitor.Core.Contracts.ResourceTypes.Enums;

namespace Promitor.Core.Contracts.ResourceTypes
{
    public class PostgreSqlResourceDefinition : AzureResourceDefinition
    {
        public PostgreSqlResourceDefinition(string subscriptionId, string resourceGroupName, string serverName, PostgreSqlServerType type)
            : base(ResourceType.PostgreSql, subscriptionId, resourceGroupName, $"{serverName}-{type}")
        {
            ServerName = serverName;
            Type = type;
        }

        public string ServerName { get; }
        public PostgreSqlServerType Type { get; }
    }
}
