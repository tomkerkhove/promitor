namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Web App.
    /// </summary>
    public class WebAppResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Web App.
        /// </summary>
        public string WebAppName { get; set; }

        /// <summary>
        /// The name of the deployment slot.
        /// </summary>
        public string SlotName { get; set; }
    }
}
