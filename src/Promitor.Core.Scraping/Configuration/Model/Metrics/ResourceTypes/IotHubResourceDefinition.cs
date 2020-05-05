namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class IotHubResourceDefinition : AzureResourceDefinition
    {
        public IotHubResourceDefinition(string subscriptionId, string resourceGroupName, string iotHubName)
          : base(ResourceType.IotHub, subscriptionId, resourceGroupName)
        {
            IotHubName = iotHubName;
        }

        public string IotHubName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => IotHubName;
    }
}