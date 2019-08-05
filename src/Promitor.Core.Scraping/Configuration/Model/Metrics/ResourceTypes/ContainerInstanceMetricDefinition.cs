using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ContainerInstanceMetricDefinition : MetricDefinition
    {
        public ContainerInstanceMetricDefinition()
        {
        }

        public ContainerInstanceMetricDefinition(AzureMetricConfiguration azureMetricConfiguration, string description, string name, string resourceGroupName, string containerGroup, Dictionary<string, string> labels, Scraping scraping)
            : base(azureMetricConfiguration, description, name, resourceGroupName, labels, scraping)
        {
            ContainerGroup = containerGroup;
        }

        public string ContainerGroup { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.ContainerInstance;
    }
}