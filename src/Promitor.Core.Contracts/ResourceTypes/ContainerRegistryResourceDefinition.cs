namespace Promitor.Core.Contracts.ResourceTypes
{
    public class ContainerRegistryResourceDefinition : AzureResourceDefinition
    {
        public ContainerRegistryResourceDefinition(string subscriptionId, string resourceGroupName, string registryName)
            : base(ResourceType.ContainerRegistry, subscriptionId, resourceGroupName, registryName)
        {
            RegistryName = registryName;
        }

        public string RegistryName { get; }
    }
}