namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    public class StorageQueueResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The storage queue account name.
        /// </summary>
        public string AccountName { get; set; }

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
