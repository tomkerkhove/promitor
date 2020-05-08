namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorConfiguration
    {
        public AzureMonitorLoggingConfiguration Logging { get; set; } = new AzureMonitorLoggingConfiguration();
    }
}