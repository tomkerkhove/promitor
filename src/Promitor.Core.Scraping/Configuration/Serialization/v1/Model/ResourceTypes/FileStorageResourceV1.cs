namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    ///     Contains the configuration required to scrape Azure Storage files.
    /// </summary>
    public class FileStorageResourceV1 : StorageAccountResourceV1
    {
        public FileStorageResourceV1()
        {
        }

        public FileStorageResourceV1(StorageAccountResourceV1 storageAccountResource)
            : base(storageAccountResource?.AccountName, storageAccountResource?.ResourceGroupName)
        {
        }
    }
}