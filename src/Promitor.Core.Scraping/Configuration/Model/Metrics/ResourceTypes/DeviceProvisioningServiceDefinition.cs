namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class DeviceProvisioningServiceDefinition : AzureResourceDefinition
    {
        public DeviceProvisioningServiceDefinition(string subscriptionId, string resourceGroupName, string deviceProvisioningServiceName)
          : base(ResourceType.DeviceProvisioningService, subscriptionId, resourceGroupName)
        {
            DeviceProvisioningServiceName = deviceProvisioningServiceName;
        }

        public string DeviceProvisioningServiceName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => DeviceProvisioningServiceName;
    }
}