namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Represents an Azure SQL Managed Instance to scrape.
    /// </summary>
    public class SqlManagedInstanceResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure SQL Managed Instance resource.
        /// </summary>
        public string InstanceName { get; set; }
    }
}