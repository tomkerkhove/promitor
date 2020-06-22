namespace Promitor.Core.Contracts.ResourceTypes
{
    public class StorageQueueResourceDefinition : AzureResourceDefinition
    {
        public StorageQueueResourceDefinition(string subscriptionId, string resourceGroupName, string accountName, string queueName, Secret sasToken)
            : base(ResourceType.StorageQueue, subscriptionId, resourceGroupName, accountName, $"{accountName}-{queueName}")
        {
            AccountName = accountName;
            QueueName = queueName;
            SasToken = sasToken;
        }

        public string AccountName { get; }
        public string QueueName { get; }
        public Secret SasToken { get; }
    }
}