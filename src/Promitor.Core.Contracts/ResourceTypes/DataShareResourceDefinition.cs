namespace Promitor.Core.Contracts.ResourceTypes
{
    public class DataShareResourceDefinition : AzureResourceDefinition
    {
        public DataShareResourceDefinition(string subscriptionId, string resourceGroupName, string shareName)
            : base(ResourceType.DataShare, subscriptionId, resourceGroupName, shareName)
        {
            ShareName = shareName;
        }

        public string ShareName { get; }
    }
}