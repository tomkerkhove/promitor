namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Function App.
    /// </summary>
    public class FunctionAppResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Function App to get metrics for.
        /// </summary>
        public string FunctionAppName { get; set; }

        /// <summary>
        /// The name of the deployment slot.
        /// </summary>
        public string SlotName { get; set; }
    }
}
