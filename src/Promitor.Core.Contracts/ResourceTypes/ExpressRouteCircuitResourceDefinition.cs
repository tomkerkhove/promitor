namespace Promitor.Core.Contracts.ResourceTypes
{
    public class ExpressRouteCircuitResourceDefinition : AzureResourceDefinition
    {
        public ExpressRouteCircuitResourceDefinition(string subscriptionId, string resourceGroupName, string expressRouteCircuitName)
            : base(ResourceType.ExpressRouteCircuit, subscriptionId, resourceGroupName, expressRouteCircuitName)
        {
            ExpressRouteCircuitName = expressRouteCircuitName;
        }

        public string ExpressRouteCircuitName { get; }
    }
}
