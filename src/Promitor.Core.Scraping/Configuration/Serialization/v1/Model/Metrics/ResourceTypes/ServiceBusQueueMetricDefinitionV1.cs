using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class ServiceBusQueueMetricDefinitionV1 : MetricDefinitionV1
    {
        public string Namespace { get; set; }
        public string QueueName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.ServiceBusQueue;
        public override MetricDefinition Build()
        {
            return new ServiceBusQueueMetricDefinition(
                AzureMetricConfiguration.Build(),
                Description,
                Name,
                ResourceGroupName,
                Namespace,
                QueueName,
                Labels,
                Scraping.Build());
        }
    }
}