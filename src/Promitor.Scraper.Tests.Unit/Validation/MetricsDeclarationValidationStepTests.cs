using System.ComponentModel;
using Promitor.Scraper.Tests.Unit.Builders;
using Promitor.Scraper.Tests.Unit.Stubs;
using Promitor.Scraper.Validation.Steps;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation
{
    [Category(category: "Unit")]
    public class MetricsDeclarationValidationStepTests
    {
        [Fact]
        public void MetricsDeclaration_DeclarationWithDuplicateMetricNames_ValidationFails()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .WithServiceBusMetric(metricName)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithMetadataThatDoesNotHaveResourceGroupName_ValidationFails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata(resourceGroupName: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithMetadataThatDoesNotHaveSubscriptionId_ValidationFails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata(subscriptionId: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithMetadataThatDoesNotHaveTenantId_ValidationFails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata(string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithoutAzureMetricName_Succeeds()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(azureMetricName: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithoutMetadata_ValidationFails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithoutMetadata()
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithoutMetricDescription_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricDescription: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, userMessage: "Validation was not successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithoutMetricName_ValidationFails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithoutQueueName_ValidationFails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(queueName: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithoutServiceBusNamespace_ValidationFails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(serviceBusNamespace: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_ValidDeclaration_Succeeds()
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
            Assert.True(validationResult.IsSuccessful, userMessage: "Validation was not successful");
        }
    }
}