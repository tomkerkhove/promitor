using GuardNet;
using Promitor.Integrations.AzureMonitor.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;

namespace Promitor.Integrations.AzureMonitor.Logging
{
    public class AzureMonitorLoggingFilter : ILogEventFilter
    {
        private readonly AzureMonitorLoggingConfiguration _azureMonitorLoggingConfiguration;

        public AzureMonitorLoggingFilter(AzureMonitorLoggingConfiguration azureMonitorLoggingConfiguration)
        {
            Guard.NotNull(azureMonitorLoggingConfiguration, nameof(azureMonitorLoggingConfiguration));

            _azureMonitorLoggingConfiguration = azureMonitorLoggingConfiguration;
        }

        public bool IsEnabled(LogEvent logEvent)
        {
            var filterMonitorLogger = Matching.FromSource<AzureMonitorIntegrationLogger>();
            var isMonitorIntegrationMessage = filterMonitorLogger(logEvent);
            if (isMonitorIntegrationMessage)
            {
                return _azureMonitorLoggingConfiguration.IsEnabled;
            }

            return true;
        }
    }
}