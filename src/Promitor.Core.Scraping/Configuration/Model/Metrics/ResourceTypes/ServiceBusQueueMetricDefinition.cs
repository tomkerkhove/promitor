using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ServiceBusQueueMetricDefinition : MetricDefinition
    {
        public ServiceBusQueueMetricDefinition()
        {
        }

        public ServiceBusQueueMetricDefinition(AzureMetricConfiguration azureMetricConfiguration, string description, string name, string resourceGroupName, string ns, string queueName, Dictionary<string, string> labels, Scraping scraping)
            : base(azureMetricConfiguration, description, name, resourceGroupName, labels, scraping)
        {
            Namespace = ns;
            QueueName = queueName;
        }

        public string Namespace { get; set; }
        public string QueueName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.ServiceBusQueue;
    }
}