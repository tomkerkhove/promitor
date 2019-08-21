using System.ComponentModel;
using AutoMapper;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Scraper.Host.Validation.Steps;
using Promitor.Scraper.Tests.Unit.Stubs;
using Xunit;
using MetricsDeclarationBuilder = Promitor.Scraper.Tests.Unit.Builders.Metrics.v1.MetricsDeclarationBuilder;

namespace Promitor.Scraper.Tests.Unit.Validation.Metrics
{
    [Category("Unit")]
    public class GeneralMetricsDeclarationValidationStepTests
    {
        private readonly IMapper _mapper;

        public GeneralMetricsDeclarationValidationStepTests()
        {
            var mapperConfiguration = new MapperConfiguration(c => c.AddProfile<V2MappingProfile>());
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact]
        public void MetricsDeclaration_DeclarationWithDuplicateMetricNames_Fails()
        {
            // Arrange
            const string metricName = "my_metric";
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithServiceBusMetric(metricName)
                .WithServiceBusMetric(metricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

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
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

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
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

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
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

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
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

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
                .WithDefaults(new MetricDefaultsV2())
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }
    }
}