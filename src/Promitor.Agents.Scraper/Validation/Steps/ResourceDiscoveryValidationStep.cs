using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;

namespace Promitor.Agents.Scraper.Validation.Steps
{
    public class ResourceDiscoveryValidationStep : ValidationStep, IValidationStep
    {
        private const string NoDiscoveryConfiguredError = "Resource discovery groups are defined in your metrics configuration, but resource discovery has not been configured in the runtime configuration. Please add a resource discovery configuration for Promitor Scraper runtime.";
        private readonly IOptions<ResourceDiscoveryConfiguration> _resourceDiscoveryConfiguration;
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;

        public ResourceDiscoveryValidationStep(IOptions<ResourceDiscoveryConfiguration> resourceDiscoveryResourceDiscoveryConfiguration, IMetricsDeclarationProvider metricsDeclarationProvider, ILogger<ResourceDiscoveryValidationStep> logger) : base( logger)
        {
            _metricsDeclarationProvider = metricsDeclarationProvider;
            _resourceDiscoveryConfiguration = resourceDiscoveryResourceDiscoveryConfiguration;
        }

        public string ComponentName { get; } = "Resource Discovery";

        public ValidationResult Run()
        {
            if (_resourceDiscoveryConfiguration?.Value == null || _resourceDiscoveryConfiguration.Value.IsConfigured == false)
            {
                var doesDeclareResourceDiscoveryGroups = DetermineIfDiscoveryGroupsAreDefined();
                if (doesDeclareResourceDiscoveryGroups)
                {
                    return ValidationResult.Failure(ComponentName, new List<string> { NoDiscoveryConfiguredError });
                }

                return ValidationResult.Successful(ComponentName);
            }

            var errorMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(_resourceDiscoveryConfiguration.Value.Host))
            {
                errorMessages.Add("No host name for resource discovery was configured");
            }
            
            if (_resourceDiscoveryConfiguration.Value.Port <= 0)
            {
                errorMessages.Add($"No valid port ({_resourceDiscoveryConfiguration.Value.Port}) for resource discovery was configured");
            }

            return errorMessages.Any() ? ValidationResult.Failure(ComponentName, errorMessages) : ValidationResult.Successful(ComponentName);
        }

        private bool DetermineIfDiscoveryGroupsAreDefined()
        {
            var metricsDeclaration = _metricsDeclarationProvider.Get(applyDefaults: true);
            return metricsDeclaration.Metrics.Any(metricDefinition => metricDefinition.ResourceDiscoveryGroups?.Count >= 1);
        }
    }
}