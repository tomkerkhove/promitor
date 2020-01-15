namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    /// <summary>
    /// Represents an Azure SQL Managed Instance resource.
    /// </summary>
    public class SqlManagedInstanceResourceDefinition : AzureResourceDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlManagedInstanceResourceDefinition" /> class.
        /// </summary>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="instanceName">The name of the Azure SQL Managed Instance resource.</param>
        public SqlManagedInstanceResourceDefinition(string resourceGroupName, string instanceName)
            : base(ResourceType.SqlManagedInstance, resourceGroupName)
        {
            InstanceName = instanceName;
        }

        /// <summary>
        /// The name of the Azure SQL Managed Instance resource.
        /// </summary>
        public string InstanceName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => InstanceName;
    }
}
