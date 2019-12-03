﻿using System;
using GuardNet;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Core.Configuration.Model.Telemetry.Sinks;
using Promitor.Core.Telemetry.Interfaces;

namespace Promitor.Core.Telemetry
{
    public class ApplicationInsightsTelemetry : IExceptionTracker
    {
        private readonly ILogger _logger;
        private readonly IOptionsMonitor<ApplicationInsightsConfiguration> _applicationInsightsConfiguration;
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsTelemetry(IOptionsMonitor<ApplicationInsightsConfiguration> applicationInsightsConfiguration, ILogger<ApplicationInsightsTelemetry> logger)
        {
            Guard.NotNull(applicationInsightsConfiguration, nameof(applicationInsightsConfiguration));
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;
            _applicationInsightsConfiguration = applicationInsightsConfiguration;
            _telemetryClient = ConfigureTelemetryClient(applicationInsightsConfiguration);
        }

        private TelemetryClient ConfigureTelemetryClient(IOptionsMonitor<ApplicationInsightsConfiguration> configuration)
        {
            var telemetryConfiguration = new TelemetryConfiguration();

            if (configuration.CurrentValue.IsEnabled)
            {
                telemetryConfiguration.DisableTelemetry = false;

                var instrumentationKey = configuration.CurrentValue.InstrumentationKey;
                if (string.IsNullOrWhiteSpace(instrumentationKey))
                {
                    throw new Exception("Application Insights instrumentation key is required when it is enabled.");
                }

                telemetryConfiguration.InstrumentationKey = instrumentationKey;
            }

            return new TelemetryClient(telemetryConfiguration);
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