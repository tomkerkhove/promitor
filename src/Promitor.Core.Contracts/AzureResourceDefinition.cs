using Newtonsoft.Json;

namespace Promitor.Core.Contracts
{
    /// <summary>
    ///     Describes a resource in Azure that can be scraped. Inheriting classes can add whatever
    ///     additional information is required to scrape a particular Azure resource.
    /// </summary>
    public class AzureResourceDefinition : IAzureResourceDefinition
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="resourceType">Type of resource that is configured</param>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">Specify a resource group to scrape that defers from the default resource group.</param>
        /// <param name="resourceName">Name of the main resource</param>
        public AzureResourceDefinition(ResourceType resourceType, string subscriptionId, string resourceGroupName, string resourceName)
            : this(resourceType, subscriptionId, resourceGroupName, resourceName, resourceName)
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="resourceType">Type of resource that is configured</param>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">Specify a resource group to scrape that defers from the default resource group.</param>
        /// <param name="resourceName">Name of the main resource</param>
        /// <param name="uniqueName">Unique name for the resource.</param>
        [JsonConstructor]
        public AzureResourceDefinition(ResourceType resourceType, string subscriptionId, string resourceGroupName, string resourceName, string uniqueName)
        {
            UniqueName = uniqueName;
            ResourceName = resourceName;
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