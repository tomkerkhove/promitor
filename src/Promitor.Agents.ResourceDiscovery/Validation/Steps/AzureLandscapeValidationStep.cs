using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Agents.ResourceDiscovery.Validation.Steps
{
    public class AzureLandscapeValidationStep : ValidationStep,
        IValidationStep
    {
        private readonly AzureLandscape _azureLandscape;

        public AzureLandscapeValidationStep(IOptions<AzureLandscape> azureLandscape, ILogger<AzureLandscapeValidationStep> logger) : base(logger)
        {
            _azureLandscape = azureLandscape.Value;
        }

        public string ComponentName { get; } = "Azure Landscape";

        public ValidationResult Run()
        {
            var errorMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(_azureLandscape.TenantId))
            {
                errorMessages.Add("No tenant id was configured");
            }

            if (_azureLandscape.Cloud == AzureCloud.Unspecified)
            {
                errorMessages.Add("No Azure cloud was configured");
            }

            if (_azureLandscape.Subscriptions == null || _azureLandscape.Subscriptions.Any() == false)
            {
                errorMessages.Add("No subscription id(s) were configured to query");
            }
            else
            {
                if (_azureLandscape.Subscriptions.Distinct().Count() != _azureLandscape.Subscriptions.Count)
                {
                    errorMessages.Add("Duplicate subscription ids were configured to query");
                }

                if (_azureLandscape.Subscriptions.Any(string.IsNullOrWhiteSpace))
                {
                    errorMessages.Add("Empty subscription is configured to query");
                }
            }

            return errorMessages.Any() ? ValidationResult.Failure(ComponentName, errorMessages) : ValidationResult.Successful(ComponentName);
        }
    }
}