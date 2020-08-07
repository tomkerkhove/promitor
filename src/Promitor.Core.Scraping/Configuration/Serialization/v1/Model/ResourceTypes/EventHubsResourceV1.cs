namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape an Azure Event Hubs namespace.
    /// </summary>
    public class EventHubsResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The Azure Event Hubs topic to scrape.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// The Azure Event Hubs namespace.
        /// </summary>
        public string Namespace { get; set; }
    }
}
