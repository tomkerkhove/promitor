namespace Promitor.Core.Contracts.ResourceTypes
{
    public class VirtualMachineResourceDefinition : AzureResourceDefinition
    {
        public VirtualMachineResourceDefinition(string subscriptionId, string resourceGroupName, string virtualMachineName)
            : base(ResourceType.VirtualMachine, subscriptionId, resourceGroupName, virtualMachineName)
        {
            VirtualMachineName = virtualMachineName;
        }

        public string VirtualMachineName { get; }
    }
}