namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class VirtualMachineResourceDefinition : AzureResourceDefinition
    {
        public VirtualMachineResourceDefinition(string subscriptionId, string resourceGroupName, string virtualMachineName)
            : base(ResourceType.VirtualMachine, subscriptionId, resourceGroupName)
        {
            VirtualMachineName = virtualMachineName;
        }

        public string VirtualMachineName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => VirtualMachineName;
    }
}