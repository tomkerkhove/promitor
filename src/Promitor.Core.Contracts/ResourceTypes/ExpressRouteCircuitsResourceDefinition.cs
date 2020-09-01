namespace Promitor.Core.Contracts.ResourceTypes
{
    public class ExpressRouteCircuitsResourceDefinition : AzureResourceDefinition
    {
        public ExpressRouteCircuitsResourceDefinition(string subscriptionId, string resourceGroupName, string expressRouteCircuitsName)
            : base(ResourceType.ExpressRouteCircuits, subscriptionId, resourceGroupName, expressRouteCircuitsName)
        {
            ExpressRouteCircuitsName = expressRouteCircuitsName;
        }

        public string ExpressRouteCircuitsName { get; }
    }
}