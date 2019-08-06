namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ContainerInstanceMetricDefinition : AzureResourceDefinition
    {
        public ContainerInstanceMetricDefinition() : base(ResourceType.ContainerInstance)
        {
        }

        public ContainerInstanceMetricDefinition(string resourceGroupName, string containerGroup)
            : base(ResourceType.ContainerInstance, resourceGroupName)
        {
            ContainerGroup = containerGroup;
        }

        public string ContainerGroup { get; set; }
    }
}