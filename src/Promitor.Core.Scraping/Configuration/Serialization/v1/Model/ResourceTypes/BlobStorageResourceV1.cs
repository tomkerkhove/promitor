namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    ///     Contains the configuration required to scrape Azure Storage blobs.
    /// </summary>
    public class BlobStorageResourceV1 : StorageAccountResourceV1
    {
        public BlobStorageResourceV1(StorageAccountResourceV1 storageAccountResource)
            : base(storageAccountResource?.AccountName, storageAccountResource?.ResourceGroupName)
        {
        }
    }
}