namespace Promitor.Core.Contracts.ResourceTypes
{
    /// <summary>
    ///     Represents files in an Azure Storage account resource.
    /// </summary>
    public class FileStorageResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FileStorageResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="accountName">The name of the Azure Storage resource.</param>
        public FileStorageResourceDefinition(string subscriptionId, string resourceGroupName, string accountName)
            : base(ResourceType.FileStorage, subscriptionId, resourceGroupName, accountName)
        {
            AccountName = accountName;
        }

        /// <summary>
        ///     The name of the Azure Storage resource.
        /// </summary>
        public string AccountName { get; }
    }
}