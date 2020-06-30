namespace Promitor.Core.Contracts.ResourceTypes
{
    public class DeviceProvisioningServiceResourceDefinition : AzureResourceDefinition
    {
        public DeviceProvisioningServiceResourceDefinition(string subscriptionId, string resourceGroupName, string deviceProvisioningServiceName)
          : base(ResourceType.DeviceProvisioningService, subscriptionId, resourceGroupName, deviceProvisioningServiceName)
        {
            DeviceProvisioningServiceName = deviceProvisioningServiceName;
        }

        public string DeviceProvisioningServiceName { get; }
    }
}