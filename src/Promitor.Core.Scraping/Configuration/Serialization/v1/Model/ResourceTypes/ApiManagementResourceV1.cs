namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    ///     Contains the configuration required to scrape a API Management instance.
    /// </summary>
    public class ApiManagementResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        ///     The name of the Azure API Management resource.
        /// </summary>
        public string InstanceName { get; set; }
    }
}