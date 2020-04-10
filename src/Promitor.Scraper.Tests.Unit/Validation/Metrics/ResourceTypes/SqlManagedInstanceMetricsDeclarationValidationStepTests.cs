using Promitor.Runtime.Agents.Scraper.Validation.Steps;
using Promitor.Scraper.Tests.Unit.Stubs;
using System.ComponentModel;
using Promitor.Scraper.Tests.Unit.Builders.Metrics.v1;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Metrics.ResourceTypes
{
    [Category("Unit")]
    public class SqlManagedInstanceMetricsDeclarationValidationStepTests : MetricsDeclarationValidationStepsTests
    {
        [Fact]
        public void SqlManagedInstanceMetricsDeclaration_DeclarationWithoutAzureMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithSqlManagedInstanceMetric(azureMetricName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void SqlManagedInstanceMetricsDeclaration_DeclarationWithoutAzureMetricDescription_Succeeds()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithSqlManagedInstanceMetric(metricDescription: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, $"Validation is not successful, errors: {validationResult.Message}");
        }

        [Fact]
        public void SqlManagedInstanceMetricsDeclaration_DeclarationWithoutInstanceName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithSqlManagedInstanceMetric(instanceName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }
    }
}
