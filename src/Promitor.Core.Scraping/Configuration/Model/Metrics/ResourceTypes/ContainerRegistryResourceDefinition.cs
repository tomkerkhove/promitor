namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ContainerRegistryResourceDefinition : AzureResourceDefinition
    {
        public ContainerRegistryResourceDefinition(string subscriptionId, string resourceGroupName, string registryName)
            : base(ResourceType.ContainerRegistry, subscriptionId, resourceGroupName)
        {
            RegistryName = registryName;
        }

        public string RegistryName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => RegistryName;
    }
}