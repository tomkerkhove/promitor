namespace Promitor.Core.Contracts.ResourceTypes
{
    /// <summary>
    /// Represents an Azure DNS Zone resource.
    /// </summary>
    public class DnsZoneResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DnsZoneResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the DNS zone is in.</param>
        /// <param name="zoneName">The DNS zone name (e.g., example.com).</param>
        public DnsZoneResourceDefinition(string subscriptionId, string resourceGroupName, string zoneName)
            : base(ResourceType.DnsZone, subscriptionId, resourceGroupName, zoneName)
        {
            ZoneName = zoneName;
        }

        /// <summary>
        /// The DNS zone name.
        /// </summary>
        public string ZoneName { get; }
    }
}