using Promitor.Core.Contracts.ResourceTypes.Enums;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a Postgre SQL database.
    /// </summary>
    public class PostgreSqlResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The postgreSQL server name.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The type of server
        /// </summary>
        public PostgreSqlServerType Type { get; set; }
    }
}
