namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    ///     Represents an Azure Synapse workspace.
    /// </summary>
    public class SynapseWorkspaceResourceV1 : AzureResourceDefinitionV1
    {
        public SynapseWorkspaceResourceV1()
        {
        }

        public SynapseWorkspaceResourceV1(string workspaceName)
        {
            WorkspaceName = workspaceName;
        }

        /// <summary>
        ///     The name of the Azure Synapse workspace.
        /// </summary>
        public string WorkspaceName { get; set; }
    }
}