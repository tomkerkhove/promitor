namespace Promitor.Core.Contracts.ResourceTypes
{
    public class CdnResourceDefinition : AzureResourceDefinition
    {
        public CdnResourceDefinition(string subscriptionId, string resourceGroupName, string cdnName)
            : base(ResourceType.Cdn, subscriptionId, resourceGroupName, cdnName)
        {
            CdnName = cdnName;
        }

        public string CdnName { get; }
    }
}