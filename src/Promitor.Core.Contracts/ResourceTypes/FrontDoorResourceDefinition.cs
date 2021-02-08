namespace Promitor.Core.Contracts.ResourceTypes
{
    /// <summary>
    ///     Represents files in an Azure Front Door resource.
    /// </summary>
    public class FrontDoorResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FrontDoorResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="name">The name of the Azure Front Door resource.</param>
        public FrontDoorResourceDefinition(string subscriptionId, string resourceGroupName, string name)
            : base(ResourceType.FrontDoor, subscriptionId, resourceGroupName, name)
        {
            Name = name;
        }

        /// <summary>
        ///     The name of the Azure Front Door resource.
        /// </summary>
        public string Name { get; }
    }
}