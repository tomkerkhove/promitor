namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a virtual machine.
    /// </summary>
    public class AzureFirewallResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the virtual machine to get metrics for.
        /// </summary>
        public string AzureFirewallName { get; set; }
    }
}
