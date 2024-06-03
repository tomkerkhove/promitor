namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorIntegrationConfiguration
    {
        public AzureMonitorHistoryConfiguration History { get; set; } = new();
        public bool UseAzureMonitorSdk { get; set; } = true;
    }
}