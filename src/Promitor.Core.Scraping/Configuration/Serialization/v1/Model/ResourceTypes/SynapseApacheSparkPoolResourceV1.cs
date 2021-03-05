namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    ///     Represents an Apache Spark pool in an Azure Synapse workspace.
    /// </summary>
    public class SynapseApacheSparkPoolResourceV1 : AzureResourceDefinitionV1
    {
        public SynapseApacheSparkPoolResourceV1()
        {
        }

        public SynapseApacheSparkPoolResourceV1(string workspaceName, string poolName)
        {
            WorkspaceName = workspaceName;
            PoolName = poolName;
        }

        /// <summary>
        ///     The name of the Azure Synapse workspace.
        /// </summary>
        public string WorkspaceName { get; set; }

        /// <summary>
        ///     The name of the Apache Spark pool inside the Synapse workspace.
        /// </summary>
        public string PoolName { get; set; }
    }
}