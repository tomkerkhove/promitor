namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Application Gateway.
    /// </summary>
    public class ApplicationGatewayResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Application Gateway to get metrics for.
        /// </summary>
        public string ApplicationGatewayName { get; set; }
    }
}
