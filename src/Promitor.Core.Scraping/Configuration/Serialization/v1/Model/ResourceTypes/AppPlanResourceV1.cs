namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a app plan.
    /// </summary>
    public class AppPlanResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure App Plan to get metrics for.
        /// </summary>
        public string AppPlanName { get; set; }
    }
}
