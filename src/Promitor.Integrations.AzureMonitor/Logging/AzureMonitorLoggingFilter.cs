using GuardNet;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;

namespace Promitor.Integrations.AzureMonitor.Logging
{
    public class AzureMonitorLoggingFilter : ILogEventFilter
    {
        private readonly AzureMonitorLoggingOptions _azureMonitorLoggingOptions;

        public AzureMonitorLoggingFilter(AzureMonitorLoggingOptions azureMonitorLoggingOptions)
        {
            Guard.NotNull(azureMonitorLoggingOptions, nameof(azureMonitorLoggingOptions));

            _azureMonitorLoggingOptions = azureMonitorLoggingOptions;
        }

        public bool IsEnabled(LogEvent logEvent)
        {
            var filterMonitorLogger = Matching.FromSource<AzureMonitorIntegrationLogger>();
            var isMonitorIntegrationMessage = filterMonitorLogger(logEvent);
            if (isMonitorIntegrationMessage)
            {
                return _azureMonitorLoggingOptions.IsEnabled;
            }

            return true;
        }
    }
}