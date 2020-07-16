namespace Promitor.Core.Contracts.ResourceTypes
{
    public class ContainerInstanceResourceDefinition : AzureResourceDefinition
    {
        public ContainerInstanceResourceDefinition(string subscriptionId, string resourceGroupName, string containerGroup)
            : base(ResourceType.ContainerInstance, subscriptionId, resourceGroupName, containerGroup)
        {
            ContainerGroup = containerGroup;
        }

        public string ContainerGroup { get; }
    }
}