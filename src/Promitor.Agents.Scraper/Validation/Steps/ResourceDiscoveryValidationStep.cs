using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Validation.Interfaces;

namespace Promitor.Agents.Scraper.Validation.Steps
{
    public class ResourceDiscoveryValidationStep : ValidationStep, IValidationStep
    {
        private readonly ResourceDiscoveryConfiguration _configuration;

        public ResourceDiscoveryValidationStep(ResourceDiscoveryConfiguration configuration) : this(configuration, NullLogger.Instance)
        {
        }

        public ResourceDiscoveryValidationStep(ResourceDiscoveryConfiguration configuration, ILogger logger) : base( logger)
        {
            _configuration = configuration;
        }

        public string ComponentName { get; } = "Resource Discovery";

        public ValidationResult Run()
        {
            if (_configuration == null)
            {
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
    }
}