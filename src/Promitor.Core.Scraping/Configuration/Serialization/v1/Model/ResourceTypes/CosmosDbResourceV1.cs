namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a Cosmos database.
    /// </summary>
    public class CosmosDbResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The cosmos database name.
        /// </summary>
        public string DbName { get; set; }
    }
}
