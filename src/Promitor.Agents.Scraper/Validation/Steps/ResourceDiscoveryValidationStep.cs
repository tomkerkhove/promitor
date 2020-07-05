using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Validation.Interfaces;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Configuration.Serialization;

namespace Promitor.Agents.Scraper.Validation.Steps
{
    public class ResourceDiscoveryValidationStep : ValidationStep, IValidationStep
    {
        private const string NoDiscoveryConfiguredError = "Resource discovery groups are defined in your metrics configuration, but resource discovery has not been configured in the runtime configuration. Please add a resource discovery configuration for Promitor Scraper runtime.";
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly ResourceDiscoveryConfiguration _configuration;

        public ResourceDiscoveryValidationStep(ResourceDiscoveryConfiguration configuration, IMetricsDeclarationProvider metricsDeclarationProvider) : this(configuration, metricsDeclarationProvider, NullLogger.Instance)
        {
        }

        public ResourceDiscoveryValidationStep(ResourceDiscoveryConfiguration configuration, IMetricsDeclarationProvider metricsDeclarationProvider, ILogger logger) : base( logger)
        {
            _metricsDeclarationProvider = metricsDeclarationProvider;
            _configuration = configuration;
        }

        public string ComponentName { get; } = "Resource Discovery";

        public ValidationResult Run()
        {
            var doesDeclareResourceDiscoveryGroups = DetermineIfDiscoveryGroupsAreDefined();
            if (_configuration == null)
            {
                if (doesDeclareResourceDiscoveryGroups)
                {
                    return ValidationResult.Failure(ComponentName, new List<string> { NoDiscoveryConfiguredError });
                }

                return ValidationResult.Successful(ComponentName);
            }

            var errorMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(_configuration.Host))
            {
                errorMessages.Add( "No host name for resource discovery was configured");
            }

            if (_configuration.Port <= 0)
            {
                errorMessages.Add($"No valid port ({_configuration.Port}) for resource discovery was configured");
            }

            return errorMessages.Any() ? ValidationResult.Failure(ComponentName, errorMessages) : ValidationResult.Successful(ComponentName);
        }

        private bool DetermineIfDiscoveryGroupsAreDefined()
        {
            var errorReporter = new ErrorReporter();
            var metricsDeclaration = _metricsDeclarationProvider.Get(applyDefaults: true, errorReporter: errorReporter);
            return metricsDeclaration.Metrics.Any(metricDefinition => metricDefinition.ResourceDiscoveryGroups?.Count >= 1);
        }
    }
}