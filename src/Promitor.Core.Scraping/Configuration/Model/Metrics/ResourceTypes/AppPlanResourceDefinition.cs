namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class AppPlanResourceDefinition : AzureResourceDefinition
    {
        public AppPlanResourceDefinition(string subscriptionId, string resourceGroupName, string appPlanName)
            : base(ResourceType.AppPlan, subscriptionId, resourceGroupName)
        {
            AppPlanName = appPlanName;
        }

        /// <summary>
        /// The name of the Azure App Plan to get metrics for.
        /// </summary>
        public string AppPlanName { get; set; }

        /// <inheritdoc />
        public override string GetResourceName() => AppPlanName;
    }
}