using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class StorageQueueMetricDefinitionBuilder : MetricDefinitionBuilder
    {
        public string AccountName { get; set; }
        public string QueueName { get; set; }
        public SecretBuilder SasToken { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.StorageQueue;

        public override MetricDefinition Build()
        {
            return new StorageQueueMetricDefinition(
                AzureMetricConfigurationBuilder.Build(),
                Description,
                Name,
                ResourceGroupName,
                AccountName,
                QueueName,
                SasToken.Build(),
                Labels,
                ScrapingBuilder.Build());
        }
    }
}