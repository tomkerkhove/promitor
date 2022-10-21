namespace Promitor.Core.Contracts.ResourceTypes
{
    public class LogAnalyticsResourceDefinition : AzureResourceDefinition
    {
        public LogAnalyticsResourceDefinition(string subscriptionId, string resourceGroupName, string workspaceName, string workspaceId)
            : base(ResourceType.LogAnalytics, subscriptionId, resourceGroupName, workspaceName)
        {
            WorkspaceName = workspaceName;
            WorkspaceId = workspaceId;
        }

        public string WorkspaceId { get; }
        public string WorkspaceName { get; }
    }
}