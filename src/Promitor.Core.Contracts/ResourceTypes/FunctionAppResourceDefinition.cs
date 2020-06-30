namespace Promitor.Core.Contracts.ResourceTypes
{
    public class FunctionAppResourceDefinition : AzureResourceDefinition, IAppServiceResourceDefinition
    {
        public FunctionAppResourceDefinition(string subscriptionId, string resourceGroupName, string functionAppName, string slotName)
            : base(ResourceType.FunctionApp, subscriptionId, resourceGroupName, functionAppName, $"{functionAppName}-{slotName}")
        {
            FunctionAppName = functionAppName;
            SlotName = slotName;
        }

        /// <summary>
        /// The name of the Azure Function App to get metrics for.
        /// </summary>
        public string FunctionAppName { get; }

        /// <summary>
        /// The name of the deployment slot.
        /// </summary>
        public string SlotName { get; }
    }
}