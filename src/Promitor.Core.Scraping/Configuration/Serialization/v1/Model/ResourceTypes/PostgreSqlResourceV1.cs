namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a Postgre SQL database.
    /// </summary>
    public class PostgreSqlResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The postgre server name.
        /// </summary>
        public string ServerName { get; set; }
    }
}
