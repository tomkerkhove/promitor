namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class WebAppResourceDefinition : AzureResourceDefinition, IAppServiceResourceDefinition
    {
        public WebAppResourceDefinition(string resourceGroupName, string webAppName, string slotName)
            : base(ResourceType.WebApp, resourceGroupName)
        {
            WebAppName = webAppName;
            SlotName = slotName;
        }

        /// <summary>
        /// The name of the Azure Web App.
        /// </summary>
        public string WebAppName { get; set; }

        /// <summary>
        /// The name of the deployment slot.
        /// </summary>
        public string SlotName { get; set; }

        /// <inheritdoc />
        public override string GetResourceName() => WebAppName;
    }
}