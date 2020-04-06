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
        /// <param name="subscriptionId">Specify a subscription to scrape that defers from the default subscription.</param>
        /// <param name="resourceGroupName">The name of the resource group the server is in.</param>
        /// <param name="instanceName">The name of the Azure SQL Managed Instance resource.</param>
        public SqlManagedInstanceResourceDefinition(string subscriptionId, string resourceGroupName, string instanceName)
            : base(ResourceType.SqlManagedInstance, subscriptionId, resourceGroupName)
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
