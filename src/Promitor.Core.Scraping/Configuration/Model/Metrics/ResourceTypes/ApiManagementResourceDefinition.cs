namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    /// <summary>
    ///     Represents an Azure API Management resource.
    /// </summary>
    public class ApiManagementResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiManagementResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="instanceName">The name of the Azure API Management resource.</param>
        public ApiManagementResourceDefinition(string subscriptionId, string resourceGroupName, string instanceName)
            : base(ResourceType.ApiManagement, subscriptionId, resourceGroupName)
        {
            InstanceName = instanceName;
        }

        /// <summary>
        ///     The name of the Azure API Management to get metrics for.
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        ///     Name of the location.
        /// </summary>
        public string LocationName { get; set; }

        /// <inheritdoc />
        public override string GetResourceName() => InstanceName;
    }
}