namespace Promitor.Core.Configuration.Model.AzureMonitor
{
    public class AzureMonitorConfiguration
    {
        public AzureMonitorLoggingConfiguration Logging { get; set; } = new AzureMonitorLoggingConfiguration();
    }
}