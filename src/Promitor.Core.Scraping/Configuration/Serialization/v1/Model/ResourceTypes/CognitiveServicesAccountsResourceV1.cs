namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Cognitive Services Accounts.
    /// </summary>
    public class CognitiveServicesAccountsResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure firewall to get metrics for.
        /// </summary>
        public string CognitiveServicesAccountsName { get; set; }
    }
}
