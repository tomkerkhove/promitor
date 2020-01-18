namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a storage queue.
    /// </summary>
    public class StorageQueueResourceV1 : StorageAccountResourceV1
    {
        public StorageQueueResourceV1()
        {
        }

        public StorageQueueResourceV1(StorageAccountResourceV1 storageAccountResource)
            : base(storageAccountResource?.AccountName, storageAccountResource?.ResourceGroupName)
        {
        }

        /// <summary>
        /// The name of the queue.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// The SAS token for accessing the queue.
        /// </summary>
        public SecretV1 SasToken { get; set; }
    }
}
