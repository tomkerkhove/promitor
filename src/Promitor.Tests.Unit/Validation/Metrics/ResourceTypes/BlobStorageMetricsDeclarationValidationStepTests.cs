using System.ComponentModel;
using Promitor.Integrations.AzureStorage;
using Promitor.Agents.Scraper.Validation.Steps;
using Promitor.Tests.Unit.Builders.Metrics.v1;
using Promitor.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Metrics.ResourceTypes
{
    [Category(category: "Unit")]
    public class BlobStorageMetricsDeclarationValidationStepTests : MetricsDeclarationValidationStepsTests
    {
        [Fact]
        public void BlobStorageMetricsDeclaration_DeclarationWithoutAccountName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithBlobStorageMetric(accountName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void BlobStorageMetricsDeclaration_DeclarationWithoutAzureMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithBlobStorageMetric(azureMetricName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void BlobStorageMetricsDeclaration_DeclarationWithoutMetricDescription_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithBlobStorageMetric(metricDescription: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, userMessage: "Validation was not successful");
        }

        [Fact]
        public void BlobStorageMetricsDeclaration_DeclarationWithoutMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithBlobStorageMetric(string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void BlobStorageMetricsDeclaration_ValidDeclarationWithAzureMetricNameInDifferentCasing_Succeeds()
        {
            // Arrange
            var azureMetricName = AzureStorageConstants.Queues.Metrics.MessageCount.ToUpper();
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithBlobStorageMetric(azureMetricName: azureMetricName)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, userMessage: "Validation was not successful");
        }

        [Fact]
        public void BlobStorageMetricsDeclaration_ValidDeclarationWithMessageCount_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithBlobStorageMetric()
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, userMessage: "Validation was not successful");
        }

        [Fact]
        public void BlobStorageMetricsDeclaration_ValidDeclarationWithTimeSpentInQueue_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithBlobStorageMetric(azureMetricName: AzureStorageConstants.Queues.Metrics.TimeSpentInQueue)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, userMessage: "Validation was not successful");
        }
    }
}