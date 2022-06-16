namespace Promitor.Core.Contracts.ResourceTypes
{
    /// <summary>
    /// Represents an Azure Database for MariaDb.
    /// </summary>
    public class MariaDbResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="serverName">The name of the Azure Database for MariaDb instance.</param>
        public MariaDbResourceDefinition(string subscriptionId, string resourceGroupName, string serverName)
            : base(ResourceType.MariaDb, subscriptionId, resourceGroupName, serverName)
        {
            ServerName = serverName;
        }

        /// <summary>
        /// The name of the MariaDb server instance.
        /// </summary>
        public string ServerName { get; }
    }
}