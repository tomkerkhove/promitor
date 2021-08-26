namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a Data Factory instance.
    /// </summary>
    public class DataFactoryResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The data factory name.
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// The data pipeline name.
        /// </summary>
        public string PipelineName { get; set; }
    }
}
