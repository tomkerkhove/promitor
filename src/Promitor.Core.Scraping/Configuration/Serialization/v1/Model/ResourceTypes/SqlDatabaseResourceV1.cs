namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Represents an Azure SQL Database to scrape.
    /// </summary>
    public class SqlDatabaseResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the SQL Server instance.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The name of the SQL database.
        /// </summary>
        public string DatabaseName { get; set; }
    }
}