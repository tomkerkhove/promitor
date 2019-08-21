namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    public class ServiceBusQueueResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The service bus queue to scrape.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// The service bus namespace.
        /// </summary>
        public string Namespace { get; set; }
    }
}
