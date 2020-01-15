namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class StorageQueueResourceDefinition : AzureResourceDefinition
    {
        public StorageQueueResourceDefinition(string resourceGroupName, string accountName, string queueName, Secret sasToken)
            : base(ResourceType.StorageQueue, resourceGroupName)
        {
            AccountName = accountName;
            QueueName = queueName;
            SasToken = sasToken;
        }

        public string AccountName { get; }
        public string QueueName { get; }
        public Secret SasToken { get; }

        /// <inheritdoc />
        public override string GetResourceName() => AccountName;
    }
}