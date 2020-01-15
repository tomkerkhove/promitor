namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class VirtualMachineScaleSetResourceDefinition : AzureResourceDefinition
    {
        public VirtualMachineScaleSetResourceDefinition(string resourceGroupName, string scaleSetName)
            : base(ResourceType.VirtualMachineScaleSet, resourceGroupName)
        {
            ScaleSetName = scaleSetName;
        }

        public string ScaleSetName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => ScaleSetName;
    }
}