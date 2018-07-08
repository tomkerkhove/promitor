using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Promitor.Scraper.Configuration;
using Promitor.Scraper.Telemetry.Interfaces;

namespace Promitor.Scraper.Telemetry
{
    public class ApplicationInsightsTelemetry : IExceptionTracker
    {
        private readonly TelemetryClient telemetryClient;

        public ApplicationInsightsTelemetry()
        {
            var instrumentationKey =
                Environment.GetEnvironmentVariable(EnvironmentVariables.Telemetry.InstrumentationKey);
            var telemetryConfiguration = new TelemetryConfiguration
            {
                InstrumentationKey = instrumentationKey,
                DisableTelemetry = false
            };
            telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        public void Track(Exception exception)
        {
            telemetryClient.TrackException(exception);
        }
    }
}