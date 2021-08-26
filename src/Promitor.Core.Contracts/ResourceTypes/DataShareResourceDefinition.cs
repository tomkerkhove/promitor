namespace Promitor.Core.Contracts.ResourceTypes
{
    public class DataShareResourceDefinition : AzureResourceDefinition
    {
        public DataShareResourceDefinition(string subscriptionId, string resourceGroupName, string accountName, string shareName)
            : base(ResourceType.DataShare, subscriptionId, resourceGroupName, $"{accountName}-{shareName}")
        {
            AccountName = accountName;
            ShareName = shareName;
        }

        public string AccountName { get; }
        public string ShareName { get; }
    }
}