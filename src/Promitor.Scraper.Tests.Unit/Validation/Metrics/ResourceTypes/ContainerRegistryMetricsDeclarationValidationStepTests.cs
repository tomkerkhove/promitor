using System.ComponentModel;
using AutoMapper;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Scraper.Host.Validation.Steps;
using Promitor.Scraper.Tests.Unit.Builders.Metrics.v1;
using Promitor.Scraper.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Metrics.ResourceTypes
{
    [Category("Unit")]
    public class ContainerRegistryMetricsDeclarationValidationStepTests
    {
        private IMapper _mapper;

        public ContainerRegistryMetricsDeclarationValidationStepTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void ContainerRegistryMetricsDeclaration_DeclarationWithoutAzureMetricName_Succeeds()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric(azureMetricName: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void ContainerRegistryMetricsDeclaration_DeclarationWithoutMetricDescription_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric(metricDescription: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, "Validation was not successful");
        }

        [Fact]
        public void

            ContainerRegistryMetricsDeclaration_DeclarationWithoutMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric(string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void ContainerRegistryMetricsDeclaration_DeclarationWithoutRegistryName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric(registryName: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void ContainerRegistryMetricsDeclaration_ValidDeclaration_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithContainerRegistryMetric()
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, "Validation was not successful");
        }
    }
}