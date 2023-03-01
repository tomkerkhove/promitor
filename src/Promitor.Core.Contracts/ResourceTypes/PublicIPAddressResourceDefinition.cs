namespace Promitor.Core.Contracts.ResourceTypes
{
    public class PublicIPAddressResourceDefinition : AzureResourceDefinition
    {
        public PublicIPAddressResourceDefinition(string subscriptionId, string resourceGroupName, string publicIPAddressName)
            : base(ResourceType.PublicIPAddress, subscriptionId, resourceGroupName, publicIPAddressName)
        {
            PublicIPAddressName = publicIPAddressName;
        }

        public string PublicIPAddressName { get; }
    }
}
