using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Agents.Scraper.Configuration;

namespace Promitor.Agents.Scraper.Validation.Steps.Sinks
{
    public class OpenTelemetryCollectorMetricSinkValidationStep : ValidationStep, IValidationStep
    {
        private readonly IOptions<ScraperRuntimeConfiguration> _runtimeConfiguration;
        public static bool TryParseProtocol(string value, out OtlpExportProtocol result)
        {
            switch (value?.Trim().ToLower())
            {
                case "grpc":
                    result = OtlpExportProtocol.Grpc;
                    return true;
                case "http/protobuf":
                    result = OtlpExportProtocol.HttpProtobuf;
                    return true;
                case "httpprotobuf":
                    result = OtlpExportProtocol.HttpProtobuf;
                    return true;
                case "http":
                    result = OtlpExportProtocol.HttpProtobuf;
                    return true;
                default:
                    result = default;
                    return false;
            }
        }

        public OpenTelemetryCollectorMetricSinkValidationStep(IOptions<ScraperRuntimeConfiguration> runtimeConfiguration, ILogger<OpenTelemetryCollectorMetricSinkValidationStep> validationLogger)
            : base(validationLogger)
        {
            _runtimeConfiguration = runtimeConfiguration;
        }

        public string ComponentName => "OpenTelemetry Collector Metric Sink";

        public ValidationResult Run()
        {
            var currentRuntimeConfiguration = _runtimeConfiguration.Value;
            var openTelemetryCollectorSinkConfiguration = currentRuntimeConfiguration.MetricSinks?.OpenTelemetryCollector;
            if (openTelemetryCollectorSinkConfiguration == null)
            {
                return ValidationResult.Successful(ComponentName);
            }

            var errorMessages = new List<string>();
            var collectorUri = openTelemetryCollectorSinkConfiguration.CollectorUri;
            var collectorProtocol = openTelemetryCollectorSinkConfiguration.Protocol.ToString();
            // Url Validation
            if (string.IsNullOrWhiteSpace(collectorUri))
            {
                errorMessages.Add("No URI for the OpenTelemetry Collector is configured.");
            }
            else
            {
                if (collectorUri.StartsWith("http://") == false && collectorUri.StartsWith("https://") == false)
                {
                    errorMessages.Add($"Configured URI ({collectorUri}) for the OpenTelemetry Collector should use HTTP or HTTPS.");
                }
                else if (Uri.TryCreate(collectorUri, UriKind.Absolute, out _) == false)
                {
                    errorMessages.Add($"Configured URI ({collectorUri}) for the OpenTelemetry Collector is not a valid URI.");
                }
            }
            // Protocol Validation
            if (!TryParseProtocol(collectorProtocol, out var parsedProtocol))
            {
                errorMessages.Add($"Invalid Protocol ({collectorProtocol}) for the OpenTelemetry Collector is configured. Please check here for valid protocols: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/README.md#otlpexporteroptions");
            }

            return errorMessages.Any() ? ValidationResult.Failure(ComponentName, errorMessages) : ValidationResult.Successful(ComponentName);
        }
    }
}
