namespace Promitor.Core.Contracts.ResourceTypes
{
    public class PublicIpAddressResourceDefinition : AzureResourceDefinition
    {
        public PublicIpAddressResourceDefinition(string subscriptionId, string resourceGroupName, string publicIpAddressName)
            : base(ResourceType.PublicIpAddress, subscriptionId, resourceGroupName, publicIpAddressName)
        {
            PublicIpAddressName = publicIpAddressName;
        }

        public string PublicIpAddressName { get; }
    }
}
