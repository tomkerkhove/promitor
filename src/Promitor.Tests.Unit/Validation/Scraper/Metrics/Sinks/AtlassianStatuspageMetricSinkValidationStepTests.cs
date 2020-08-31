using System;
using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Agents.Scraper.Validation.Steps.Sinks;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Integrations.Sinks.Atlassian.Statuspage.Configuration;
using Promitor.Tests.Unit.Builders.Metrics.v1;
using Promitor.Tests.Unit.Generators.Config;
using Promitor.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Scraper.Metrics.Sinks
{
    [Category("Unit")]
    public class AtlassianStatuspageMetricSinkValidationStepTests
    {
        private readonly IMapper _mapper;

        public AtlassianStatuspageMetricSinkValidationStepTests()
        {
            var mapperConfiguration = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact]
        public void Validate_AtlassianStatuspageIsFullyConfigured_Success()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName);

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspageIsNotConfigured_Success()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName);
            runtimeConfiguration.Value.MetricSinks.AtlassianStatuspage = null;

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }
        [Fact]
        public void Validate_NoSinksConfigured_Success()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName);
            runtimeConfiguration.Value.MetricSinks = null;

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspageWithEmptyPageId_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName: metricName);
            runtimeConfiguration.Value.MetricSinks.AtlassianStatuspage.PageId = string.Empty;

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspageWithoutPageId_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName: metricName);
            runtimeConfiguration.Value.MetricSinks.AtlassianStatuspage.PageId = null;

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspageWithoutSystemMetricId_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName: metricName);
            runtimeConfiguration.Value.MetricSinks.AtlassianStatuspage.SystemMetricMapping[0].Id = null;

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspageWithEmptySystemMetricId_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName: metricName);
            runtimeConfiguration.Value.MetricSinks.AtlassianStatuspage.SystemMetricMapping[0].Id = string.Empty;

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspageWithoutPromitorMetricName_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName: metricName);
            runtimeConfiguration.Value.MetricSinks.AtlassianStatuspage.SystemMetricMapping[0].PromitorMetricName = null;

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspageWithEmptyPromitorMetricName_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName: metricName);
            runtimeConfiguration.Value.MetricSinks.AtlassianStatuspage.SystemMetricMapping[0].PromitorMetricName = string.Empty;

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspageWithoutSystemMetricMapping_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName);
            runtimeConfiguration.Value.MetricSinks.AtlassianStatuspage.SystemMetricMapping = null;

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspageWithDuplicateIdsInSystemMetricMapping_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var systemMetricMapping = new SystemMetricMapping
            {
                Id = Guid.NewGuid().ToString(),
                PromitorMetricName = metricName
            };
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName);
            runtimeConfiguration.Value.MetricSinks.AtlassianStatuspage.SystemMetricMapping.Clear();
            runtimeConfiguration.Value.MetricSinks.AtlassianStatuspage.SystemMetricMapping.Add(systemMetricMapping);
            runtimeConfiguration.Value.MetricSinks.AtlassianStatuspage.SystemMetricMapping.Add(systemMetricMapping);

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspageWithUnmappedSystemMetricMapping_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName: "other_metric");

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspageWithPromitorMetricUsingResourceDiscovery_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            const string resourceDiscoveryGroupName = "my_discovery_group";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(resourceDiscoveryGroupName: resourceDiscoveryGroupName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName);

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void Validate_AtlassianStatuspagePromitorMetricScrapingMultipleResources_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            List<string> queueNames = new List<string> { "queue-1", "queue-2"};
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(queueNames: queueNames)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);
            var runtimeConfiguration = CreateRuntimeConfiguration(metricName);

            // Act
            var azureAuthenticationValidationStep = new AtlassianStatuspageMetricSinkValidationStep(runtimeConfiguration, metricsDeclarationProvider, NullLogger<AtlassianStatuspageMetricSinkValidationStep>.Instance);
            var validationResult = azureAuthenticationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        private IOptions<ScraperRuntimeConfiguration> CreateRuntimeConfiguration(string metricName, string pageId = null, string systemMetricId = null)
        {
            var bogusRuntimeConfiguration = BogusScraperRuntimeConfigurationGenerator.Generate();
            bogusRuntimeConfiguration.MetricSinks.AtlassianStatuspage.SystemMetricMapping.Clear();

            if (string.IsNullOrWhiteSpace(pageId) == false)
            {
                bogusRuntimeConfiguration.MetricSinks.AtlassianStatuspage.PageId = pageId;
            }

            var systemMetricMapping = new Faker<SystemMetricMapping>()
                .StrictMode(true)
                .RuleFor(map => map.Id, faker => systemMetricId ?? faker.Person.FirstName)
                .RuleFor(map => map.PromitorMetricName, faker => metricName?? faker.Person.FirstName)
                .Generate();

            bogusRuntimeConfiguration.MetricSinks.AtlassianStatuspage.SystemMetricMapping.Add(systemMetricMapping);

            return Options.Create(bogusRuntimeConfiguration);
        }
    }
}