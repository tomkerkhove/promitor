namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class FunctionAppResourceDefinition : AzureResourceDefinition
    {
        public FunctionAppResourceDefinition(string resourceGroupName, string functionAppName)
            : base(ResourceType.FunctionApp, resourceGroupName)
        {
            FunctionAppName = functionAppName;
        }

        /// <summary>
        /// The name of the Azure Function App to get metrics for.
        /// </summary>
        public string FunctionAppName { get; set; }
    }
}