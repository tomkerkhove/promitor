namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class VirtualMachineResourceDefinition : AzureResourceDefinition
    {
        public VirtualMachineResourceDefinition() : base(ResourceType.VirtualMachine)
        {
        }

        public VirtualMachineResourceDefinition(string resourceGroupName, string virtualMachineName)
            : base(ResourceType.VirtualMachine, resourceGroupName)
        {
            VirtualMachineName = virtualMachineName;
        }

        public string VirtualMachineName { get; set; }
    }
}