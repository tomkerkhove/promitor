namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Monitor Autoscale settings.
    /// </summary>
    public class MonitorAutoscaleResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The name of the Azure Monitor Autoscale settings to get metrics for.
        /// </summary>
        public string AutoscaleSettingsName { get; set; }
    }
}
