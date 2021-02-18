namespace Promitor.Core.Contracts.ResourceTypes
{
    /// <summary>
    /// Represents an Azure SQL Elastic Pool resource.
    /// </summary>
    public class SqlElasticPoolResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlElasticPoolResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="serverName">The name of the Azure SQL Server instance.</param>
        /// <param name="poolName">The name of the Azure SQL Elastic Pool instance.</param>
        public SqlElasticPoolResourceDefinition(string subscriptionId, string resourceGroupName, string serverName, string poolName)
            : base(ResourceType.SqlElasticPool, subscriptionId, resourceGroupName, poolName, $"{serverName}-{poolName}")
        {
            ServerName = serverName;
            PoolName = poolName;
        }

        /// <summary>
        /// The name of the SQL server instance.
        /// </summary>
        public string ServerName { get; }

        /// <summary>
        /// The name of the Azure SQL Elastic Pool instance.
        /// </summary>
        public string PoolName { get; }
    }
}