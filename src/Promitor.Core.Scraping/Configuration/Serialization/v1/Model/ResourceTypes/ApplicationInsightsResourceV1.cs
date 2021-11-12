namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Application Insights.
    /// </summary>
    public class ApplicationInsightsResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Application Insights instance to get metrics for.
        /// </summary>
        public string Name { get; set; }
    }
}
