namespace Promitor.Core.Contracts.ResourceTypes
{
	public class PowerBiEmbeddedResourceDefinition : AzureResourceDefinition
    {
        public PowerBiEmbeddedResourceDefinition(string subscriptionId, string resourceGroupName, string capacityName)
            : base(ResourceType.PowerBiEmbedded, subscriptionId, resourceGroupName, capacityName)
        {
            CapacityName = capacityName;
        }

        public string CapacityName { get; }
    }
}