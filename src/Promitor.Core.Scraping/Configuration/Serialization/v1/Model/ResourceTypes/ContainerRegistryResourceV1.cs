namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a Container Registry.
    /// </summary>
    public class ContainerRegistryResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the registry to scrape.
        /// </summary>
        public string RegistryName { get; set; }
    }
}
