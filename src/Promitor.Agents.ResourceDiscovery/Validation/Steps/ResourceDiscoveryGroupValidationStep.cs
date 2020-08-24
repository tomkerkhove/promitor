using System.Collections.Generic;
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
        private readonly IOptions<List<ResourceDiscoveryGroup>> _resourceDiscoveryGroups;

        public ResourceDiscoveryGroupValidationStep(IOptions<List<ResourceDiscoveryGroup>> resourceDiscoveryGroups, ILogger<ResourceDiscoveryGroupValidationStep> logger) : base(logger)
        {
            _resourceDiscoveryGroups = resourceDiscoveryGroups;
        }

        public string ComponentName { get; } = "Resource Discovery Groups";

        public ValidationResult Run()
        {
            return ValidationResult.Successful(ComponentName);
        }
    }
}