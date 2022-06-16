using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Agents.Scraper.Validation.Steps;
using Promitor.Tests.Unit.Builders.Metrics.v1;
using Promitor.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Scraper.Metrics.ResourceTypes
{
    [Category("Unit")]
    public class RedisEnterpriseCacheMetricsDeclarationValidationStepTests : MetricsDeclarationValidationStepsTests
    {
        [Fact]
        public void RedisEnterpriseCacheMetricsDeclaration_DeclarationWithoutAzureMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithRedisEnterpriseCacheMetric(azureMetricName: string.Empty)
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
        public void RedisEnterpriseCacheMetricsDeclaration_DeclarationWithInvalidMetricLimit_Fails(int metricLimit)
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithRedisEnterpriseCacheMetric(azureMetricLimit: metricLimit)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void RedisEnterpriseCacheMetricsDeclaration_DeclarationWithoutMetricDescription_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithRedisEnterpriseCacheMetric(metricDescription: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void RedisEnterpriseCacheMetricsDeclaration_DeclarationWithoutMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithRedisEnterpriseCacheMetric(string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void RedisEnterpriseCacheMetricsDeclaration_DeclarationWithoutRedisEnterpriseCacheName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithRedisEnterpriseCacheMetric(cacheName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void RedisEnterpriseCacheMetricsDeclaration_DeclarationWithoutResourceAndResourceDiscoveryGroupInfo_Fails()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithRedisEnterpriseCacheMetric(omitResource: true)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void RedisEnterpriseCacheMetricsDeclaration_DeclarationWithoutResourceButWithResourceDiscoveryGroupInfo_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithRedisEnterpriseCacheMetric(omitResource: true, resourceDiscoveryGroupName:"sample-collection")
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void RedisEnterpriseCacheMetricsDeclaration_ValidDeclaration_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithRedisEnterpriseCacheMetric()
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