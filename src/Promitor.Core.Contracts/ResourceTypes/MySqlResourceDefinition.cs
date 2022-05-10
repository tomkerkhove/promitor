using Promitor.Core.Contracts.ResourceTypes.Enums;

namespace Promitor.Core.Contracts.ResourceTypes
{
    public class MySqlResourceDefinition : AzureResourceDefinition
    {
        public MySqlResourceDefinition(string subscriptionId, string resourceGroupName, string serverName, MySqlServerType type)
            : base(ResourceType.MySql, subscriptionId, resourceGroupName, $"{serverName}-{type}")
        {
            ServerName = serverName;
            Type = type;
        }

        public string ServerName { get; }
        public MySqlServerType Type { get; }
    }
}
