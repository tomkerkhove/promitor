using System.Collections.Generic;
using System.ComponentModel;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Validation.Steps;
using Promitor.Core.Contracts;
using Xunit;

namespace Promitor.Tests.Unit.Validation.ResourceDiscovery
{
    [Category("Unit")]
    public class ResourceDiscoveryGroupValidationStepTests
    {
        [Fact]
        public void Validate_ResourceDiscoveryGroupIsFullyConfigured_Success()
        {
            // Arrange
            var resourceDiscoveryGroupConfiguration = CreateResourceDiscoveryGroupConfiguration();

            // Act
            var resourceDiscoveryGroupValidationStep = new ResourceDiscoveryGroupValidationStep(resourceDiscoveryGroupConfiguration, NullLogger<ResourceDiscoveryGroupValidationStep>.Instance);
            var validationResult = resourceDiscoveryGroupValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful);
        }

        private IOptions<List<ResourceDiscoveryGroup>> CreateResourceDiscoveryGroupConfiguration(int resourceDiscoveryGroupAmount = 2)
        {
            var groups = new List<ResourceDiscoveryGroup>();

            for (var count = 1; count <= resourceDiscoveryGroupAmount; count++)
            {
                var resourceDiscoveryGroup = GenerateResourceDiscoveryGroup();
                groups.Add(resourceDiscoveryGroup);
            }

            return Options.Create(groups);
        }

        private static ResourceDiscoveryGroup GenerateResourceDiscoveryGroup()
        {
            var azureLandscape = new Faker<ResourceDiscoveryGroup>()
                .StrictMode(true)
                .RuleFor(group => group.Name, faker => faker.Name.FirstName())
                .RuleFor(group => group.Type, faker => faker.PickRandom<ResourceType>())
                .Generate();
            return azureLandscape;
        }
    }
}