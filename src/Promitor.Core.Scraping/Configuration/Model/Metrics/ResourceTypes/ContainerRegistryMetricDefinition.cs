namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ContainerRegistryMetricDefinition : AzureResourceDefinition
    {
        public ContainerRegistryMetricDefinition() : base(ResourceType.ContainerRegistry)
        {
        }

        public ContainerRegistryMetricDefinition(string resourceGroupName, string registryName)
            : base(ResourceType.ContainerRegistry, resourceGroupName)
        {
            RegistryName = registryName;
        }

        public string RegistryName { get; set; }
    }
}