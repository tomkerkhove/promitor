using System.ComponentModel;
using Promitor.Scraper.Host.Validation.Steps;
using Promitor.Scraper.Tests.Unit.Builders.Metrics.v1;
using Promitor.Scraper.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Metrics
{
    [Category("Unit")]
    public class GeneralMetricsDeclarationValidationStepTests
    {
        [Fact]
        public void MetricsDeclaration_DeclarationWithDuplicateMetricNames_Fails()
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
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithMetadataThatDoesNotHaveResourceGroupName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata(resourceGroupName: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithMetadataThatDoesNotHaveSubscriptionId_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata(subscriptionId: string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithMetadataThatDoesNotHaveTenantId_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata(string.Empty)
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithoutMetadata_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithoutMetadata()
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void MetricsDeclaration_WithoutDefaultScrapingSchedule_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithDefaults(new Core.Scraping.Configuration.Model.MetricDefaults())
                .Build();
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }
    }
}