namespace Promitor.Core.Contracts.ResourceTypes
{
    public class PowerBiDedicatedResourceDefinition : AzureResourceDefinition
    {
        public PowerBiDedicatedResourceDefinition(string subscriptionId, string resourceGroupName, string capacityName) 
		: base(ResourceType.PowerBiDedicated, subscriptionId, resourceGroupName, capacityName)
        {
            CapacityName = capacityName;
        }
        public string CapacityName { get; }
    }
}
