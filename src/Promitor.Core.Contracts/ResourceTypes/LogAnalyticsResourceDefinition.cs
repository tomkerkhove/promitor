namespace Promitor.Core.Contracts.ResourceTypes
{
    public class LogAnalyticsResourceDefinition : AzureResourceDefinition
    {
        public LogAnalyticsResourceDefinition(string subscriptionId, string resourceGroupName, string name, string workspaceId)
            : base(ResourceType.LogAnalytics, subscriptionId, resourceGroupName, name)
        {
            Name = name;
            WorkspaceId = workspaceId;
        }

        public string WorkspaceId { get; }
        public string Name { get; }
    }
}