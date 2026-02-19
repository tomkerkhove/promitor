namespace Promitor.Core.Contracts.ResourceTypes
{
    public class SearchServiceResourceDefinition : AzureResourceDefinition
    {
        public SearchServiceResourceDefinition(string subscriptionId, string resourceGroupName, string searchServiceName)
            : base(ResourceType.SearchService, subscriptionId, resourceGroupName, searchServiceName)
        {
            SearchServiceName = searchServiceName;
        }

        public string SearchServiceName { get; }
    }
}
