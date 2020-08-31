using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Agents.ResourceDiscovery.Configuration;

namespace Promitor.Agents.ResourceDiscovery.Validation.Steps
{
    public class ResourceDiscoveryGroupValidationStep : ValidationStep,
        IValidationStep
    {
        private readonly List<ResourceDiscoveryGroup> _resourceDiscoveryGroups;

        public ResourceDiscoveryGroupValidationStep(IOptions<List<ResourceDiscoveryGroup>> resourceDiscoveryGroups, ILogger<ResourceDiscoveryGroupValidationStep> logger) : base(logger)
        {
            _resourceDiscoveryGroups = resourceDiscoveryGroups.Value;
        }

        public string ComponentName { get; } = "Resource Discovery Groups";

        public ValidationResult Run()
        {
            var errorMessages = new List<string>();
            if (_resourceDiscoveryGroups == null || _resourceDiscoveryGroups.Any() == false)
            {
                errorMessages.Add("No resource discovery groups were configured");
                return ValidationResult.Failure(ComponentName, errorMessages);
            }

            var uniqueDiscoveryGroups = _resourceDiscoveryGroups.Select(grp=>grp.Name).Distinct().ToList();
            if (_resourceDiscoveryGroups.Count != uniqueDiscoveryGroups.Count)
            {
                foreach (var discoveryGroup in uniqueDiscoveryGroups)
                {
                    var duplicateEntryCount = _resourceDiscoveryGroups.Count(grp => grp.Name.Equals(discoveryGroup, StringComparison.InvariantCultureIgnoreCase));
                    if (duplicateEntryCount > 1)
                    {
                        errorMessages.Add($"Duplicate resource discovery groups were configured with name '{discoveryGroup}'. ({duplicateEntryCount} groups with same name found)");
                        return ValidationResult.Failure(ComponentName, errorMessages);
                    }
                }
            }

            return ValidationResult.Successful(ComponentName);
        }
    }
}