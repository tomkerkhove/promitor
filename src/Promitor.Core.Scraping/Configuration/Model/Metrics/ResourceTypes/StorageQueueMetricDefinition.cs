using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class StorageQueueMetricDefinition : MetricDefinition
    {
        public StorageQueueMetricDefinition()
        {
        }

        public StorageQueueMetricDefinition(AzureMetricConfiguration azureMetricConfiguration, string description, string name, string resourceGroupName, string accountName, string queueName, Secret sasToken, Dictionary<string, string> labels, Scraping scraping)
            : base(azureMetricConfiguration, description, name, resourceGroupName, labels, scraping)
        {
            AccountName = accountName;
            QueueName = queueName;
            SasToken = sasToken;
        }

        public string AccountName { get; set; }
        public string QueueName { get; set; }
        public Secret SasToken { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.StorageQueue;
    }
}