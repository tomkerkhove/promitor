namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class IoTHubResourceDefinition : AzureResourceDefinition
    {
        public IoTHubResourceDefinition(string subscriptionId, string resourceGroupName, string iotHubName)
          : base(ResourceType.IoTHub, subscriptionId, resourceGroupName)
        {
            IoTHubName = iotHubName;
        }

        public string IoTHubName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => IoTHubName;
    }
}