using Promitor.Core.Contracts.ResourceTypes.Enums;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a MySQL database.
    /// </summary>
    public class MySqlResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The MySQL server name.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The type of server
        /// </summary>
        public MySqlServerType Type { get; set; }
    }
}
