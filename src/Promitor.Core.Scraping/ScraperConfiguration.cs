using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.FeatureFlags;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Telemetry.Interfaces;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Core.Scraping
{
    public class ScraperConfiguration
    {
        /// <summary>
        ///     Metadata concerning the Azure resources
        /// </summary>
        public AzureMetadata AzureMetadata { get; }

        /// <summary>
        ///     Client to interact with Azure Monitor
        /// </summary>
        public AzureMonitorClient AzureMonitorClient { get; }

        /// <summary>
        ///     Interaction with feature flags
        /// </summary>
        public FeatureToggleClient FeatureToggleClient { get; }

        /// <summary>
        ///     Logger used for telemetry
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        ///     Used for exception tracking
        /// </summary>
        public IExceptionTracker ExceptionTracker { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="azureMetadata">Metadata concerning the Azure resources</param>
        /// <param name="azureMonitorClient">Client to communicate with Azure Monitor</param>
        /// <param name="featureToggleClient">Interaction with feature flags</param>
        /// <param name="logger">General logger</param>
        /// <param name="exceptionTracker">Exception tracker</param>
        public ScraperConfiguration(AzureMetadata azureMetadata, AzureMonitorClient azureMonitorClient, FeatureToggleClient featureToggleClient, ILogger logger, IExceptionTracker exceptionTracker)
        {
            Guard.NotNull(azureMetadata, nameof(azureMetadata));
            Guard.NotNull(azureMonitorClient, nameof(azureMonitorClient));
            Guard.NotNull(featureToggleClient, nameof(featureToggleClient));
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(exceptionTracker, nameof(exceptionTracker));

            AzureMetadata = azureMetadata;
            AzureMonitorClient = azureMonitorClient;
            FeatureToggleClient = featureToggleClient;
            Logger = logger;
            ExceptionTracker = exceptionTracker;
        }
    }
}