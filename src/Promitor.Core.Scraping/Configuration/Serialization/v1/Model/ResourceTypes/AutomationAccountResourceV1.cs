namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    public class AutomationAccountResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        ///     The name of the Azure Automation account to get metrics for.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        ///     The name of the runbook in your Azure Automation Account account to get metrics for.
        /// </summary>
        public string RunbookName { get; set; }
    }
}