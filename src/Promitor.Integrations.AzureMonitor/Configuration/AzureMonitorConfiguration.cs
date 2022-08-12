namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorConfiguration
    {
        public AzureMonitorIntegrationConfiguration Integration { get; set; } = new AzureMonitorIntegrationConfiguration();
        public AzureMonitorLoggingConfiguration Logging { get; set; } = new AzureMonitorLoggingConfiguration();
    }
}