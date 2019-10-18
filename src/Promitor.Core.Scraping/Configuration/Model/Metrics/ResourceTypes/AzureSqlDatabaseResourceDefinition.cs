namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    /// <summary>
    /// Represents an Azure SQL Database resource.
    /// </summary>
    public class AzureSqlDatabaseResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureSqlDatabaseResourceDefinition" /> class.
        /// </summary>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="serverName">The name of the Azure SQL Server instance.</param>
        /// <param name="databaseName">The name of the Azure SQL database name.</param>
        public AzureSqlDatabaseResourceDefinition(string resourceGroupName, string serverName, string databaseName)
            : base(ResourceType.AzureSqlDatabase, resourceGroupName)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
        }

        /// <summary>
        /// The name of the SQL server instance.
        /// </summary>
        public string ServerName { get; }

        /// <summary>
        /// The name of the database.
        /// </summary>
        public string DatabaseName { get; }
    }
}