namespace Promitor.Core.Contracts.ResourceTypes
{
    public class AutomationAccountResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiManagementResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="automationAccountName">The name of the Azure Automation account resource.</param>
        public AutomationAccountResourceDefinition(string subscriptionId, string resourceGroupName, string automationAccountName)
        : base(ResourceType.AutomationAccount, subscriptionId, resourceGroupName, automationAccountName)
        {
        }

        /// <summary>
        ///     The name of the Azure Automation Account account to get metrics for.
        /// </summary>
        public string AccountName { get; }
    }
}
