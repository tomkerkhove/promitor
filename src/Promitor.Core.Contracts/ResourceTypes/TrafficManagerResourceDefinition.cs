namespace Promitor.Core.Contracts.ResourceTypes
{
    /// <summary>
    ///     Represents an Azure Traffic manager profile resource.
    /// </summary>
    public class TrafficManagerResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrafficManagerResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="name">The name of the Azure Traffic manager profile resource.</param>
        public TrafficManagerResourceDefinition(string subscriptionId, string resourceGroupName, string name)
            : base(ResourceType.TrafficManager, subscriptionId, resourceGroupName, name)
        {
            Name = name;
        }

        /// <summary>
        ///     The name of the Azure Traffic manager profile resource.
        /// </summary>
        public string Name { get; }
    }
}