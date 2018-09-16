using System.ComponentModel;
using Promitor.Scraper.Host.Validation.Steps;
using Promitor.Scraper.Tests.Unit.Builders;
using Promitor.Scraper.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Metrics.ResourceTypes
{
    [Category("Unit")]
    public class GenericMetricsDeclarationValidationStepTests
    {
        [Fact]
        public void GenericMetricsDeclaration_DeclarationWithoutAzureMetricName_Succeeds()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithGenericMetric(azureMetricName: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void GenericMetricsDeclaration_DeclarationWithoutFilter_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithGenericMetric(filter: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void GenericMetricsDeclaration_DeclarationWithoutMetricDescription_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithGenericMetric(metricDescription: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, "Validation was not successful");
        }

        [Fact]
        public void GenericMetricsDeclaration_DeclarationWithoutMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithGenericMetric(string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void GenericMetricsDeclaration_DeclarationWithoutResourceUri_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithGenericMetric(resourceUri: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void GenericMetricsDeclaration_ValidDeclaration_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric()
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, "Validation was not successful");
        }
    }
}