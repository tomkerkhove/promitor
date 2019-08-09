namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes
{
    public class StorageQueueResourceV2 : AzureResourceDefinitionV2
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
        public SecretV2 SasToken { get; set; }
    }
}
