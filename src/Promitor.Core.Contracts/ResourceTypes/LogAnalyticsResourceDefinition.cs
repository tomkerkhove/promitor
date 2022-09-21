namespace Promitor.Core.Contracts.ResourceTypes
{
    public class LogAnalyticsResourceDefinition : AzureResourceDefinition
    {
        public LogAnalyticsResourceDefinition(string subscriptionId, string resourceGroupName, string workspaceId)
            : base(ResourceType.LogAnalytics, subscriptionId, resourceGroupName, workspaceId)
        {
            WorkspaceId = workspaceId;
        }

        public string WorkspaceId { get; }
    }
}