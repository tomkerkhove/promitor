namespace Promitor.Core.Contracts
{
    /// <summary>
    ///     Describes a resource in Azure that can be scraped. Inheriting classes can add whatever
    ///     additional information is required to scrape a particular Azure resource.
    /// </summary>
    public interface IAzureResourceDefinition
    {
        /// <summary>
        ///     Type of resource that is configured
        /// </summary>
        public ResourceType ResourceType { get; }

        /// <summary>
        ///     Specify a subscription to scrape that defers from the default subscription.
        ///     This enables you to do multi-subscription scraping with one configuration file.
        /// </summary>
        public string SubscriptionId { get; }

        /// <summary>
        ///     Specify a resource group to scrape that defers from the default resource group.
        ///     This enables you to do multi-resource group scraping with one configuration file.
        /// </summary>
        public string ResourceGroupName { get; }

        /// <summary>
        ///     Specify the name of the resource
        /// </summary>
        /// <remarks>This should return the name of the main resource</remarks>
        /// <example>
        ///     For an Azure SQL Database it should be the name of the DB, not the server
        /// </example>
        public string ResourceName { get; }

        /// <summary>
        ///     Specify a unique name for the resource.
        /// </summary>
        public string UniqueName { get; }
    }
}