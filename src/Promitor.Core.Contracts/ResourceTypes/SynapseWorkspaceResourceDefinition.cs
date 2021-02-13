namespace Promitor.Core.Contracts.ResourceTypes
{
    /// <summary>
    ///     Represents an Azure Synapse workspace.
    /// </summary>
    public class SynapseWorkspaceResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SynapseWorkspaceResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="workspaceName">The name of the Azure Synapse workspace.</param>
        public SynapseWorkspaceResourceDefinition(string subscriptionId, string resourceGroupName, string workspaceName)
            : base(ResourceType.SynapseWorkspace, subscriptionId, resourceGroupName, workspaceName)
        {
            WorkspaceName = workspaceName;
        }

        /// <summary>
        ///     The name of the Azure Synapse workspace.
        /// </summary>
        public string WorkspaceName { get; }
    }
}