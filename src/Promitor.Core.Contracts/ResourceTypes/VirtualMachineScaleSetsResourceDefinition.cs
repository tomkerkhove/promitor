namespace Promitor.Core.Contracts.ResourceTypes
{
    public class VirtualMachineScaleSetResourceDefinition : AzureResourceDefinition
    {
        public VirtualMachineScaleSetResourceDefinition(string subscriptionId, string resourceGroupName, string scaleSetName)
            : base(ResourceType.VirtualMachineScaleSet, subscriptionId, resourceGroupName, scaleSetName)
        {
            ScaleSetName = scaleSetName;
        }

        public string ScaleSetName { get; }
    }
}