namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class FunctionAppResourceDefinition : AzureResourceDefinition, IAppServiceResourceDefinition
    {
        public FunctionAppResourceDefinition(string subscriptionId, string resourceGroupName, string functionAppName)
            : base(ResourceType.FunctionApp, subscriptionId, resourceGroupName)
        {
            FunctionAppName = functionAppName;
        }

        /// <summary>
        /// The name of the Azure Function App to get metrics for.
        /// </summary>
        public string FunctionAppName { get; set; }

        /// <summary>
        /// The name of the deployment slot.
        /// </summary>
        public string SlotName { get; set; }

        /// <inheritdoc />
        public override string GetResourceName() => FunctionAppName;
    }
}