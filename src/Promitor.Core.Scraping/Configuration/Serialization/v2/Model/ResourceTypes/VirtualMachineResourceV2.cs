namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes
{
    public class VirtualMachineResourceV2 : AzureResourceDefinitionV2
    {
        /// <summary>
        /// The name of the virtual machine to get metrics for.
        /// </summary>
        public string VirtualMachineName { get; set; }
    }
}
