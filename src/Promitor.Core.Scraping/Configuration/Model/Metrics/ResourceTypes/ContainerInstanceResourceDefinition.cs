namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ContainerInstanceResourceDefinition : AzureResourceDefinition
    {
        public ContainerInstanceResourceDefinition(string resourceGroupName, string containerGroup)
            : base(ResourceType.ContainerInstance, resourceGroupName)
        {
            ContainerGroup = containerGroup;
        }

        public string ContainerGroup { get; }

        /// <inheritdoc />
        public override string GetResourceName() => ContainerGroup;
    }
}