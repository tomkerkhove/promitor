namespace Promitor.Core.Contracts.ResourceTypes
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
        /// <param name="locationName">Name of the location.</param>
        public ApiManagementResourceDefinition(string subscriptionId, string resourceGroupName, string instanceName, string locationName)
            : base(ResourceType.ApiManagement, subscriptionId, resourceGroupName, instanceName, $"{instanceName}-{locationName}")
        {
            InstanceName = instanceName;
            LocationName = locationName;
        }

        /// <summary>
        ///     The name of the Azure API Management to get metrics for.
        /// </summary>
        public string InstanceName { get; }

        /// <summary>
        ///     Name of the location.
        /// </summary>
        public string LocationName { get; }
    }
}