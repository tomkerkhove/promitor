using System.ComponentModel;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Validation.Steps;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Tests.Unit.Builders.Metrics.v1;
using Promitor.Tests.Unit.Generators.Config;
using Promitor.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Scraper.Misc
{
    [Category("Unit")]
    public class AzureLandscapeValidationStepTests
    {
        private readonly IMapper _mapper;

        public AzureLandscapeValidationStepTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Validate_ResourceDiscoveryIsFullyConfigured_Success()
        {
            // Arrange
            var metricsDeclarationProvider = GetMetricDeclarationProvider();
            var resourceDiscoveryConfiguration = CreateRuntimeConfiguration();

            // Act
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration, metricsDeclarationProvider, NullLogger<ResourceDiscoveryValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void Validate_ResourceDiscoveryConfigurationIsNotConfigured_Success()
        {
            // Arrange
            var metricsDeclarationProvider = GetMetricDeclarationProvider();
            IOptions<ResourceDiscoveryConfiguration> resourceDiscoveryConfiguration = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration, metricsDeclarationProvider, NullLogger<ResourceDiscoveryValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void Validate_ResourceDiscoveryHostIsNotConfigured_Success()
        {
            // Arrange
            var metricsDeclarationProvider = GetMetricDeclarationProvider();
            var resourceDiscoveryConfiguration = CreateRuntimeConfiguration();
            resourceDiscoveryConfiguration.Value.Host = string.Empty;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration, metricsDeclarationProvider, NullLogger<ResourceDiscoveryValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void Validate_ResourceDiscoveryConfigurationIsNotConfiguredButMetricWithDiscoveryIsDefined_Fails()
        {
            // Arrange
            var metricsDeclarationProvider = GetMetricDeclarationProvider(useDiscoveryGroup: true);
            IOptions<ResourceDiscoveryConfiguration> resourceDiscoveryConfiguration = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration, metricsDeclarationProvider, NullLogger<ResourceDiscoveryValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_NoResourceDiscoveryHostIsNotConfiguredButMetricWithDiscoveryIsDefined_Fails()
        {
            // Arrange
            var metricsDeclarationProvider = GetMetricDeclarationProvider(useDiscoveryGroup: true);
            var resourceDiscoveryConfiguration = CreateRuntimeConfiguration();
            resourceDiscoveryConfiguration.Value.Host = string.Empty;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration, metricsDeclarationProvider, NullLogger<ResourceDiscoveryValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_PortIsNegative_Fails()
        {
            // Arrange
            var metricsDeclarationProvider = GetMetricDeclarationProvider();
            var resourceDiscoveryConfiguration = CreateRuntimeConfiguration();
            resourceDiscoveryConfiguration.Value.Port = -1;

            // Act
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration, metricsDeclarationProvider, NullLogger<ResourceDiscoveryValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_PortIsZero_Fails()
        {
            // Arrange
            var metricsDeclarationProvider = GetMetricDeclarationProvider();
            var resourceDiscoveryConfiguration = CreateRuntimeConfiguration();
            resourceDiscoveryConfiguration.Value.Port = 0;

            // Act
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration, metricsDeclarationProvider, NullLogger<ResourceDiscoveryValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Validate_NoHostIsConfigured_Succeeds(string host)
        {
            // Arrange
            var metricsDeclarationProvider = GetMetricDeclarationProvider();
            var resourceDiscoveryConfiguration = CreateRuntimeConfiguration();
            resourceDiscoveryConfiguration.Value.Host = host;

            // Act
            var azureAuthenticationValidationStep = new ResourceDiscoveryValidationStep(resourceDiscoveryConfiguration, metricsDeclarationProvider, NullLogger<ResourceDiscoveryValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        private IOptions<ResourceDiscoveryConfiguration> CreateRuntimeConfiguration()
        {
            var bogusRuntimeConfiguration = BogusScraperRuntimeConfigurationGenerator.Generate();

            return Options.Create(bogusRuntimeConfiguration.ResourceDiscovery);
        }

        private MetricsDeclarationProviderStub GetMetricDeclarationProvider(bool useDiscoveryGroup=false)
        {
            var metricsDeclarationBuilder = MetricsDeclarationBuilder.WithMetadata()
                .WithApiManagementMetric(locationName: string.Empty);

            if (useDiscoveryGroup)
            {
                metricsDeclarationBuilder.WithApiManagementMetric(resourceDiscoveryGroupName: "hello-cloud");
            }

            var rawDeclaration = metricsDeclarationBuilder
                .Build(_mapper);

            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            return metricsDeclarationProvider;
        }
    }
}