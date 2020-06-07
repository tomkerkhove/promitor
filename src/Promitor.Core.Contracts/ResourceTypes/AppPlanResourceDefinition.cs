namespace Promitor.Core.Contracts.ResourceTypes
{
    public class AppPlanResourceDefinition : AzureResourceDefinition
    {
        public AppPlanResourceDefinition(string subscriptionId, string resourceGroupName, string appPlanName)
            : base(ResourceType.AppPlan, subscriptionId, resourceGroupName, appPlanName)
        {
            AppPlanName = appPlanName;
        }

        /// <summary>
        /// The name of the Azure App Plan to get metrics for.
        /// </summary>
        public string AppPlanName { get; }
    }
}