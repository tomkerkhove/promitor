namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class ServiceBusQueueResourceDefinition : AzureResourceDefinition
    {
        public ServiceBusQueueResourceDefinition(string subscriptionId, string resourceGroupName, string ns, string queueName)
            : base(ResourceType.ServiceBusQueue, subscriptionId, resourceGroupName)
        {
            Namespace = ns;
            QueueName = queueName;
        }

        public string Namespace { get; }
        public string QueueName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => QueueName;
    }
}