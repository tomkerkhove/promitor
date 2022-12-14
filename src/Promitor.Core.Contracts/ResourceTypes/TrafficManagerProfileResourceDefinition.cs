namespace Promitor.Core.Contracts.ResourceTypes
{
    /// <summary>
    ///     Represents an Azure Traffic manager profile resource.
    /// </summary>
    public class TrafficManagerProfileResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrafficManagerProfileResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="profileName">The name of the Azure Traffic manager profile resource.</param>
        public TrafficManagerProfileResourceDefinition(string subscriptionId, string resourceGroupName, string profileName)
            : base(ResourceType.TrafficManagerProfile, subscriptionId, resourceGroupName, profileName)
        {
            ProfileName = profileName;
        }

        /// <summary>
        ///     The name of the Azure Traffic manager profile resource.
        /// </summary>
        public string ProfileName { get; }
    }
}