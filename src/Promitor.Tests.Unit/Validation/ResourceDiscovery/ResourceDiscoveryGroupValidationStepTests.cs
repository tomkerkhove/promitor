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
    public class ResourceDiscoveryGroupValidationStepTests : UnitTest
    {
        [Fact]
        public void Validate_ResourceDiscoveryGroupsAreFullyConfigured_Success()
        {
            // Arrange
            var resourceDiscoveryGroupConfiguration = CreateResourceDiscoveryGroupConfiguration();

            // Act
            var resourceDiscoveryGroupValidationStep = new ResourceDiscoveryGroupValidationStep(resourceDiscoveryGroupConfiguration, NullLogger<ResourceDiscoveryGroupValidationStep>.Instance);
            var validationResult = resourceDiscoveryGroupValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void Validate_DuplicateResourceDiscoveryGroupNames_Fails()
        {
            // Arrange
            var resourceDiscoveryGroupConfiguration = CreateResourceDiscoveryGroupConfiguration(resourceDiscoveryGroupAmount: 2);
            resourceDiscoveryGroupConfiguration.Value[1].Name = resourceDiscoveryGroupConfiguration.Value[0].Name;

            // Act
            var resourceDiscoveryGroupValidationStep = new ResourceDiscoveryGroupValidationStep(resourceDiscoveryGroupConfiguration, NullLogger<ResourceDiscoveryGroupValidationStep>.Instance);
            var validationResult = resourceDiscoveryGroupValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
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

        private static ResourceDiscoveryGroup GenerateResourceDiscoveryGroup(bool includeCriteria = true)
        {
            var criteria = includeCriteria ? GenerateCriteriaDefinition() : null;
            var resourceDiscoveryGroup = new Faker<ResourceDiscoveryGroup>()
                .StrictMode(true)
                .RuleFor(group => group.Name, faker => faker.Name.FirstName())
                .RuleFor(group => group.Type, faker => faker.PickRandom<ResourceType>())
                .RuleFor(group => group.Criteria, _ => criteria)
                .Generate();

            return resourceDiscoveryGroup;
        }

        private static ResourceCriteriaDefinition GenerateCriteriaDefinition()
        {
            var criteria = GenerateCriteria();
            var criteriaDefinition = new Faker<ResourceCriteriaDefinition>()
                .StrictMode(true)
                .RuleFor(definition => definition.Include, _ => criteria)
                .Generate();
            return criteriaDefinition;
        }

        private static ResourceCriteria GenerateCriteria()
        {
            var resourceCriteria = new Faker<ResourceCriteria>()
                .StrictMode(true)
                .RuleFor(criteria => criteria.Subscriptions, faker => new List<string>{faker.Random.Guid().ToString(), faker.Random.Guid().ToString()})
                .RuleFor(criteria => criteria.Tags, faker => new Dictionary<string, string> { { faker.Name.FirstName(), faker.Random.Guid().ToString() } , { faker.Name.FirstName(), faker.Random.Guid().ToString() } })
                .RuleFor(criteria => criteria.Regions, faker => new List<string> { faker.Random.Guid().ToString(), faker.Random.Guid().ToString() })
                .RuleFor(criteria => criteria.ResourceGroups, faker => new List<string> { faker.Random.Guid().ToString(), faker.Random.Guid().ToString() })
                .Generate();
            return resourceCriteria;
        }
    }
}