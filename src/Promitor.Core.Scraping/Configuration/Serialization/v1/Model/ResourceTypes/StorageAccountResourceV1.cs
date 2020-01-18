namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    ///     Contains the configuration required to scrape an Storage account instance.
    /// </summary>
    public class StorageAccountResourceV1 : AzureResourceDefinitionV1
    {
        public StorageAccountResourceV1()
        {
        }

        protected StorageAccountResourceV1(string accountName, string resourceGroupName)
        {
            AccountName = accountName;
            ResourceGroupName = resourceGroupName;
        }

        /// <summary>
        ///     The name of the Azure Storage account resource.
        /// </summary>
        public string AccountName { get; set; }
    }
}