using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class NetworkInterfaceMetricDefinition : MetricDefinition
    {
        public NetworkInterfaceMetricDefinition()
        {
        }

        public NetworkInterfaceMetricDefinition(AzureMetricConfiguration azureMetricConfiguration, string description, string name, string resourceGroupName, string networkInterfaceName, Dictionary<string, string> labels, Scraping scraping)
            : base(name, description, resourceGroupName, labels, scraping, azureMetricConfiguration)
        {
            NetworkInterfaceName = networkInterfaceName;
        }

        public string NetworkInterfaceName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.NetworkInterface;
    }
}