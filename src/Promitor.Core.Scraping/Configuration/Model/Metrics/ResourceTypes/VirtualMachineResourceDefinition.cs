namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class VirtualMachineResourceDefinition : AzureResourceDefinition
    {
        public VirtualMachineResourceDefinition(string resourceGroupName, string virtualMachineName)
            : base(ResourceType.VirtualMachine, resourceGroupName)
        {
            VirtualMachineName = virtualMachineName;
        }

        public string VirtualMachineName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => VirtualMachineName;
    }
}