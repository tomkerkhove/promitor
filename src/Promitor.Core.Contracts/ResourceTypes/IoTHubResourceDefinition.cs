namespace Promitor.Core.Contracts.ResourceTypes
{
    public class IoTHubResourceDefinition : AzureResourceDefinition
    {
        public IoTHubResourceDefinition(string subscriptionId, string resourceGroupName, string iotHubName)
          : base(ResourceType.IoTHub, subscriptionId, resourceGroupName, iotHubName)
        {
            IoTHubName = iotHubName;
        }

        public string IoTHubName { get; }
    }
}