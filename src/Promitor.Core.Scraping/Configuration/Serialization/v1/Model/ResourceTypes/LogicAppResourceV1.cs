namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Logic App.
    /// </summary>
    public class LogicAppResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the workflow to scrape.
        /// </summary>
        public string WorkflowName { get; set; }
    }
}
