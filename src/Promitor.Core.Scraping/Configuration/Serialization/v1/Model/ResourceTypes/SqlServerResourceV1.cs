namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    ///     Contains the configuration required to scrape a SQL Server.
    /// </summary>
    public class SqlServerResourceV1 : AzureResourceDefinitionV1
    {
        public SqlServerResourceV1()
        {
        }

        protected SqlServerResourceV1(string serverName, string resourceGroupName)
        {
            ServerName = serverName;
            ResourceGroupName = resourceGroupName;
        }

        /// <summary>
        ///     The name of the Azure SQL Server resource.
        /// </summary>
        public string ServerName { get; set; }
    }
}