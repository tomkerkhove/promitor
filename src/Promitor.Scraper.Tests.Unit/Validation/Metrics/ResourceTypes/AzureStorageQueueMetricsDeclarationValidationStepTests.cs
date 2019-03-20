using System.ComponentModel;
using Promitor.Scraper.Host.Validation.Steps;
using Promitor.Scraper.Tests.Unit.Builders;
using Promitor.Scraper.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Metrics.ResourceTypes
{
    [Category("Unit")]
    public class AzureStorageQueueMetricsDeclarationValidationStepTests
{
        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutAzureMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(azureMetricName: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutMetricDescription_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(metricDescription: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, "Validation was not successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(metricName: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutQueueName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(queueName: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutAccountName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(accountName: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutSasToken_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(sasToken: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_ValidDeclaration_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric()
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