namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a virtual machine scale set.
    /// </summary>
    public class VirtualMachineScaleSetResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the scale set machine to get metrics for.
        /// </summary>
        public string ScaleSetName { get; set; }
    }
}
