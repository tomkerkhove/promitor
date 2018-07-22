using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Promitor.Core.Telemetry.Interfaces;

namespace Promitor.Core.Telemetry
{
    public class ApplicationInsightsTelemetry : IExceptionTracker
    {
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsTelemetry()
        {
            var instrumentationKey = Environment.GetEnvironmentVariable(EnvironmentVariables.Telemetry.InstrumentationKey);
            var telemetryConfiguration = new TelemetryConfiguration
            {
                DisableTelemetry = false
            };

            if (string.IsNullOrWhiteSpace(instrumentationKey) == false)
            {
                telemetryConfiguration.InstrumentationKey = instrumentationKey;
            }

            _telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        public void Track(Exception exception)
        {
            _telemetryClient.TrackException(exception);
        }
    }
}