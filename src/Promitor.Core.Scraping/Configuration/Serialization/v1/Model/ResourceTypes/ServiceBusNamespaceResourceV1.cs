namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    /// <summary>
    /// Contains the configuration required to scrape a service bus queue.
    /// </summary>
    public class ServiceBusNamespaceResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The service bus queue to scrape.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// The service bus topic to scrape.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// The service bus namespace.
        /// </summary>
        public string Namespace { get; set; }
    }
}
