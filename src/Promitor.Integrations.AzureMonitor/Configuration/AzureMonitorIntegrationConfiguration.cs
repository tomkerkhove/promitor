using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorIntegrationConfiguration
    {
        public AzureMonitorHistoryConfiguration History { get; set; } = new AzureMonitorHistoryConfiguration();
    }
}