namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Configuration required to scrape an Azure DNS Zone.
    /// </summary>
    public class DnsZoneResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The DNS zone name (e.g., example.com).
        /// </summary>
        public string ZoneName { get; set; }
    }
}