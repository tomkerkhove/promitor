﻿using System.Collections.Generic;
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
            
            if (_azureLandscape.Cloud == AzureCloud.Custom)
            {
                errorMessages.AddRange(ValidateCustomCloud());
            }
            
            if (_azureLandscape.Subscriptions == null || _azureLandscape.Subscriptions.Count == 0)
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

        private IEnumerable<string> ValidateCustomCloud()
        {
            var errorMessages = new List<string>();

            if(_azureLandscape.Endpoints == null)
            {
                errorMessages.Add("Endpoints are not configured for Azure Custom cloud");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(_azureLandscape.Endpoints.AuthenticationEndpoint))
                {
                    errorMessages.Add("Azure Custom cloud authentication endpoint was not configured to query");
                }
                if (string.IsNullOrWhiteSpace(_azureLandscape.Endpoints.ResourceManagerEndpoint))
                {
                    errorMessages.Add("Azure Custom cloud resource management endpoint was not configured to query");
                }
                if (string.IsNullOrWhiteSpace(_azureLandscape.Endpoints.ManagementEndpoint))
                {
                    errorMessages.Add("Azure Custom cloud service management endpoint was not configured to query");
                }
                if (string.IsNullOrWhiteSpace(_azureLandscape.Endpoints.GraphEndpoint))
                {
                    errorMessages.Add("Azure Custom cloud graph endpoint was not configured to query");
                }
                if (string.IsNullOrWhiteSpace(_azureLandscape.Endpoints.StorageEndpointSuffix))
                {
                    errorMessages.Add("Azure Custom cloud storage service url suffix was not configured to query");
                }
                if (string.IsNullOrWhiteSpace(_azureLandscape.Endpoints.KeyVaultSuffix))
                {
                    errorMessages.Add("Azure Custom cloud Key Vault service url suffix was not configured to query");
                }
            }

            return errorMessages;
        }
    }
}