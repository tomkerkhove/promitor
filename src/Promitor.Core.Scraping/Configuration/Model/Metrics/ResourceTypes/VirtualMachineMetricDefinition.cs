namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class VirtualMachineMetricDefinition : AzureResourceDefinition
    {
        public VirtualMachineMetricDefinition() : base(ResourceType.VirtualMachine)
        {
        }

        public VirtualMachineMetricDefinition(string resourceGroupName, string virtualMachineName)
            : base(ResourceType.VirtualMachine, resourceGroupName)
        {
            VirtualMachineName = virtualMachineName;
        }

        public string VirtualMachineName { get; set; }
    }
}