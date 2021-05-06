namespace Promitor.Core.Contracts.ResourceTypes
{
    public class MonitorAutoscaleResourceDefinition : AzureResourceDefinition
    {
        public MonitorAutoscaleResourceDefinition(string subscriptionId, string resourceGroupName, string autoscaleSettingsName)
            : base(ResourceType.MonitorAutoscale, subscriptionId, resourceGroupName, autoscaleSettingsName)
        {
            AutoscaleSettingsName = autoscaleSettingsName;
        }

        public string AutoscaleSettingsName { get; }
    }
}
