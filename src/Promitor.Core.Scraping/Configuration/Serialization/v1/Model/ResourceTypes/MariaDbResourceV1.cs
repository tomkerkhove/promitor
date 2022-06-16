namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Database for Maria DB instance.
    /// </summary>
    public class MariaDbResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Database for Maria DB server instance.
        /// </summary>
        public string ServerName { get; set; }
    }
}
