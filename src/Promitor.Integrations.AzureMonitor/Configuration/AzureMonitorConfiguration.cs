namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorConfiguration
    {
        public AzureMonitorIntegrationConfiguration Integration { get; set; } = new();
        public AzureMonitorLoggingConfiguration Logging { get; set; } = new();
    }
}