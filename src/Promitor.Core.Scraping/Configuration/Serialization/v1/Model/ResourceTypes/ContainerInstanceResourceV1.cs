namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a Container Instance.
    /// </summary>
    public class ContainerInstanceResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The container group name.
        /// </summary>
        public string ContainerGroup { get; set; }
    }
}
