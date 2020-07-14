namespace Promitor.Core.Contracts.ResourceTypes
{
    public class LogicAppResourceDefinition : AzureResourceDefinition
    {
        public LogicAppResourceDefinition(string subscriptionId, string resourceGroupName, string workflowName)
            : base(ResourceType.LogicApp, subscriptionId, resourceGroupName, workflowName)
        {
            WorkflowName = workflowName;
        }

        public string WorkflowName { get; }
    }
}