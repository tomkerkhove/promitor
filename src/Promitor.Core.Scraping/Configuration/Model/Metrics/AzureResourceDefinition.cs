namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    /// <summary>
    /// Describes a resource in Azure that can be scraped. Inheriting classes can add whatever
    /// additional information is required to scrape a particular Azure resource.
    /// </summary>
    public abstract class AzureResourceDefinition
    {
        protected AzureResourceDefinition(ResourceType resourceType, string resourceGroupName)
        {
            ResourceType = resourceType;
            ResourceGroupName = resourceGroupName;
        }

        /// <summary>
        ///     Type of resource that is configured
        /// </summary>
        public ResourceType ResourceType { get; }

        /// <summary>
        ///     Specify a resource group to scrape that defers from the default resource group.
        ///     This enables you to do multi-resource group scraping with one configuration file.
        /// </summary>
        public string ResourceGroupName { get; }
    }
}
