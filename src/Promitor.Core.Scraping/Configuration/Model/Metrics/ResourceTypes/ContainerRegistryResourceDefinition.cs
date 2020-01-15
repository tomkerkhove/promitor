namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ContainerRegistryResourceDefinition : AzureResourceDefinition
    {
        public ContainerRegistryResourceDefinition(string resourceGroupName, string registryName)
            : base(ResourceType.ContainerRegistry, resourceGroupName)
        {
            RegistryName = registryName;
        }

        public string RegistryName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => RegistryName;
    }
}