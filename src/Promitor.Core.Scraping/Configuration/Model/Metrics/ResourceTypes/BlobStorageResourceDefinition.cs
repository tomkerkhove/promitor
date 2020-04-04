namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    /// <summary>
    ///     Represents blob information in an Azure Storage account resource.
    /// </summary>
    public class BlobStorageResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BlobStorageResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="accountName">The name of the Azure Storage resource.</param>
        public BlobStorageResourceDefinition(string subscriptionId, string resourceGroupName, string accountName)
            : base(ResourceType.BlobStorage, subscriptionId, resourceGroupName)
        {
            AccountName = accountName;
        }

        /// <summary>
        ///     The name of the Azure Storage resource.
        /// </summary>
        public string AccountName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => AccountName;
    }
}