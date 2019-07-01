using System;
using GuardNet;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration.Telemetry.Sinks;
using Promitor.Core.Telemetry.Interfaces;

namespace Promitor.Core.Telemetry
{
    public class ApplicationInsightsTelemetry : IExceptionTracker
    {
        private readonly ILogger _logger;
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsTelemetry(IConfiguration configuration, ILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));

            var applicationInsightsConfiguration = configuration.GetSection("telemetry:applicationInsights").Get<ApplicationInsightsConfiguration>();
            var telemetryConfiguration = new TelemetryConfiguration
            {
                DisableTelemetry = applicationInsightsConfiguration.IsEnabled
            };

            if (string.IsNullOrWhiteSpace(applicationInsightsConfiguration.InstrumentationKey) == false)
            {
                telemetryConfiguration.InstrumentationKey = applicationInsightsConfiguration.InstrumentationKey;
            }

            _logger = logger;
            _telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        public void Track(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            _logger.LogError(exception, exception.Message);
            _telemetryClient.TrackException(exception);
        }
    }
}