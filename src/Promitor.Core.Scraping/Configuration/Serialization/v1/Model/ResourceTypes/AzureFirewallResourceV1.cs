namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure firewall.
    /// </summary>
    public class AzureFirewallResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure firewall to get metrics for.
        /// </summary>
        public string AzureFirewallName { get; set; }
    }
}
