namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    /// <summary>
    ///     Describes a resource in Azure that can be scraped. Inheriting classes can add whatever
    ///     additional information is required to scrape a particular Azure resource.
    /// </summary>
    public abstract class AzureResourceDefinition : IAzureResourceDefinition
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="resourceType">Type of resource that is configured</param>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">Specify a resource group to scrape that defers from the default resource group.</param>
        protected AzureResourceDefinition(ResourceType resourceType, string subscriptionId, string resourceGroupName)
        {
            ResourceType = resourceType;
            SubscriptionId = subscriptionId;
            ResourceGroupName = resourceGroupName;
        }

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

        /// <inheritdoc />
        public abstract string GetResourceName();
    }
}