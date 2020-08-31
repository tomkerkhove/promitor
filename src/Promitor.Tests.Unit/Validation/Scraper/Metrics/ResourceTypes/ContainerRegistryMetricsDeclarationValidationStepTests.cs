using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Agents.Scraper.Validation.Steps;
using Promitor.Tests.Unit.Builders.Metrics.v1;
using Promitor.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Scraper.Metrics.ResourceTypes
{
    [Category("Unit")]
    public class ContainerRegistryMetricsDeclarationValidationStepTests : MetricsDeclarationValidationStepsTests
    {
        [Fact]
        public void ContainerRegistryMetricsDeclaration_DeclarationWithoutAzureMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric(azureMetricName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void ContainerRegistryMetricsDeclaration_DeclarationWithoutMetricDescription_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric(metricDescription: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void ContainerRegistryMetricsDeclaration_DeclarationWithoutMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric(string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void ContainerRegistryMetricsDeclaration_DeclarationWithoutRegistryName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric(registryName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void ContainerRegistryMetricsDeclaration_DeclarationWithoutResourceAndResourceDiscoveryGroupInfo_Fails()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric(omitResource: true)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void ContainerRegistryMetricsDeclaration_DeclarationWithoutResourceButWithResourceDiscoveryGroupInfo_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric(omitResource: true, resourceDiscoveryGroupName:"sample-collection")
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void ContainerRegistryMetricsDeclaration_ValidDeclaration_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric()
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }
    }
}