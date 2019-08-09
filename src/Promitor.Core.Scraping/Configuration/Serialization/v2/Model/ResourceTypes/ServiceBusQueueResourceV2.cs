namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes
{
    public class ServiceBusQueueResourceV2 : AzureResourceDefinitionV2
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
