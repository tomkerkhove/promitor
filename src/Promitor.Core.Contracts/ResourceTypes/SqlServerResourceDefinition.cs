namespace Promitor.Core.Contracts.ResourceTypes
{
    /// <summary>
    ///     Represents an Azure SQL Server resource.
    /// </summary>
    public class SqlServerResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlServerResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="serverName">The name of the Azure SQL Server resource.</param>
        public SqlServerResourceDefinition(string subscriptionId, string resourceGroupName, string serverName)
            : base(ResourceType.SqlServer, subscriptionId, resourceGroupName,serverName)
        {
            ServerName = serverName;
        }

        /// <summary>
        ///     The name of the Azure SQL Server resource.
        /// </summary>
        public string ServerName { get; }
    }
}