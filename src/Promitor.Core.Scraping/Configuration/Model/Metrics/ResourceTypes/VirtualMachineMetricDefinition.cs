using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class VirtualMachineMetricDefinition : MetricDefinition
    {
        public VirtualMachineMetricDefinition()
        {
        }

        public VirtualMachineMetricDefinition(AzureMetricConfiguration azureMetricConfiguration, string description, string name, string resourceGroupName, string virtualMachineName, Dictionary<string, string> labels, Scraping scraping)
            : base(azureMetricConfiguration, description, name, resourceGroupName, labels, scraping)
        {
            VirtualMachineName = virtualMachineName;
        }

        public string VirtualMachineName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.VirtualMachine;
    }
}