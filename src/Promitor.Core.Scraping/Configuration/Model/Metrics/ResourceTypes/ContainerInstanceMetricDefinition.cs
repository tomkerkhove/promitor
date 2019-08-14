using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ContainerInstanceMetricDefinition : MetricDefinition
    {
        public ContainerInstanceMetricDefinition()
        {
        }

        public ContainerInstanceMetricDefinition(AzureMetricConfiguration azureMetricConfiguration, string description, string name, string resourceGroupName, string containerGroup, Dictionary<string, string> labels, Scraping scraping)
            : base(name, description, resourceGroupName, labels, scraping, azureMetricConfiguration)
        {
            ContainerGroup = containerGroup;
        }

        public string ContainerGroup { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.ContainerInstance;
    }
}