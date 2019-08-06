namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class StorageQueueResourceDefinition : AzureResourceDefinition
    {
        public StorageQueueResourceDefinition() : base(ResourceType.StorageQueue)
        {
        }

        public StorageQueueResourceDefinition(string resourceGroupName, string accountName, string queueName, Secret sasToken)
            : base(ResourceType.StorageQueue, resourceGroupName)
        {
            AccountName = accountName;
            QueueName = queueName;
            SasToken = sasToken;
        }

        public string AccountName { get; set; }
        public string QueueName { get; set; }
        public Secret SasToken { get; set; }
    }
}