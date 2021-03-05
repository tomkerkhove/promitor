namespace Promitor.Core.Contracts.ResourceTypes
{
    /// <summary>
    ///     Represents a Apache Spark pool in an Azure Synapse workspace.
    /// </summary>
    public class SynapseApacheSparkPoolResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SynapseApacheSparkPoolResourceDefinition" /> class.
        /// </summary>
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="workspaceName">The name of the Azure Synapse workspace.</param>
        /// <param name="poolName">The name of the Apache Spark pool inside the Synapse workspace.</param>
        public SynapseApacheSparkPoolResourceDefinition(string subscriptionId, string resourceGroupName, string workspaceName, string poolName)
            : base(ResourceType.SynapseApacheSparkPool, subscriptionId, resourceGroupName, poolName, $"{workspaceName}-{poolName}")
        {
            WorkspaceName = workspaceName;
            PoolName = poolName;
        }

        /// <summary>
        ///     The name of the Azure Synapse workspace.
        /// </summary>
        public string WorkspaceName { get; }

        /// <summary>
        ///     The name of the Apache Spark pool inside the Synapse workspace.
        /// </summary>
        public string PoolName { get; }
    }
}