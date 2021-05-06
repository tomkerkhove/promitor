using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Agents.Scraper.Validation.Steps;
using Promitor.Tests.Unit.Builders.Metrics.v1;
using Promitor.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Scraper.Metrics.ResourceTypes
{
    [Category("Unit")]
    public class SqlElasticPoolMetricsDeclarationValidationStepTests : MetricsDeclarationValidationStepsTests
    {
        [Fact]
        public void SqlElasticPoolMetricsDeclaration_DeclarationWithoutAzureMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithSqlElasticPoolMetric(azureMetricName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(10001)]
        public void SqlElasticPoolMetricsDeclaration_DeclarationWithInvalidMetricLimit_Fails(int metricLimit)
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithSqlElasticPoolMetric(azureMetricLimit: metricLimit)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void SqlElasticPoolMetricsDeclaration_DeclarationWithoutAzureMetricDescription_Succeeds()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithSqlElasticPoolMetric(metricDescription: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void SqlElasticPoolMetricsDeclaration_DeclarationWithoutServerName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithSqlElasticPoolMetric(serverName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void SqlElasticPoolMetricsDeclaration_DeclarationWithoutPoolName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithSqlElasticPoolMetric(poolName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void SqlElasticPoolMetricsDeclaration_DeclarationWithoutResourceAndResourceDiscoveryGroupInfo_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithSqlElasticPoolMetric(omitResource: true)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void SqlElasticPoolMetricsDeclaration_DeclarationWithoutResourceButWithResourceDiscoveryGroupInfo_Succeeds()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithSqlElasticPoolMetric(omitResource:true, resourceDiscoveryGroupName:"sample-collection")
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void SqlElasticPoolMetricsDeclaration_ValidDeclaration_Succeeds()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithSqlElasticPoolMetric()
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }
    }
}
