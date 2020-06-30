namespace Promitor.Core.Contracts.ResourceTypes
{
    public class WebAppResourceDefinition : AzureResourceDefinition, IAppServiceResourceDefinition
    {
        public WebAppResourceDefinition(string subscriptionId, string resourceGroupName, string webAppName, string slotName)
            : base(ResourceType.WebApp, subscriptionId, resourceGroupName, webAppName, $"{webAppName}-{slotName}")
        {
            WebAppName = webAppName;
            SlotName = slotName;
        }

        /// <summary>
        /// The name of the Azure Web App.
        /// </summary>
        public string WebAppName { get; }

        /// <summary>
        /// The name of the deployment slot.
        /// </summary>
        public string SlotName { get; }
    }
}