using System;
using GuardNet;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Promitor.Core.Telemetry.Interfaces;

namespace Promitor.Core.Telemetry
{
    public class ApplicationInsightsTelemetry : IExceptionTracker
    {
        private readonly ILogger _logger;
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsTelemetry(ILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));

            var instrumentationKey = Environment.GetEnvironmentVariable(EnvironmentVariables.Telemetry.InstrumentationKey);
            var telemetryConfiguration = new TelemetryConfiguration
            {
                DisableTelemetry = false
            };

            if (string.IsNullOrWhiteSpace(instrumentationKey) == false)
            {
                telemetryConfiguration.InstrumentationKey = instrumentationKey;
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