using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class StorageQueueMetricDefinitionV1 : MetricDefinitionV1
    {
        public string AccountName { get; set; }
        public string QueueName { get; set; }
        public SecretV1 SasToken { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.StorageQueue;

        public override MetricDefinition Build()
        {
            return new StorageQueueMetricDefinition(
                AzureMetricConfiguration.Build(),
                Description,
                Name,
                ResourceGroupName,
                AccountName,
                QueueName,
                SasToken.Build(),
                Labels,
                Scraping.Build());
        }
    }
}