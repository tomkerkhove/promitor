namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    ///     Represents an SQL pool in an Azure Synapse workspace.
    /// </summary>
    public class SynapseSqlPoolResourceV1 : AzureResourceDefinitionV1
    {
        public SynapseSqlPoolResourceV1()
        {
        }

        public SynapseSqlPoolResourceV1(string workspaceName, string poolName)
        {
            WorkspaceName = workspaceName;
            PoolName = poolName;
        }

        /// <summary>
        ///     The name of the Azure Synapse workspace.
        /// </summary>
        public string WorkspaceName { get; set; }

        /// <summary>
        ///     The name of the SQL pool inside the Synapse workspace.
        /// </summary>
        public string PoolName { get; set; }
    }
}