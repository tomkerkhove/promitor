using System;
using System.ComponentModel;
using AutoMapper;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Integrations.AzureStorage;
using Promitor.Scraper.Host.Validation.Steps;
using Promitor.Scraper.Tests.Unit.Builders.Metrics.v1;
using Promitor.Scraper.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Metrics.ResourceTypes
{
    [Category(category: "Unit")]
    public class AzureStorageQueueMetricsDeclarationValidationStepTests
    {
        private readonly IMapper _mapper;

        public AzureStorageQueueMetricsDeclarationValidationStepTests()
        {
            var mapperConfiguration = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithNotSupportedMetricName_Fails()
        {
            // Arrange
            var azureMetricName = Guid.NewGuid().ToString();
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(azureMetricName: azureMetricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation was successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutAccountName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(accountName: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutAzureMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(azureMetricName: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutMetricDescription_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(metricDescription: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, userMessage: "Validation was not successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutQueueName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(queueName: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_DeclarationWithoutSasToken_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(sasToken: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, userMessage: "Validation is successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_ValidDeclarationWithAzureMetricNameInDifferentCasing_Succeeds()
        {
            // Arrange
            var azureMetricName = AzureStorageConstants.Queues.Metrics.MessageCount.ToUpper();
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(azureMetricName: azureMetricName)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, userMessage: "Validation was not successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_ValidDeclarationWithMessageCount_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric()
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, userMessage: "Validation was not successful");
        }

        [Fact]
        public void AzureStorageQueuesMetricsDeclaration_ValidDeclarationWithTimeSpentInQueue_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithAzureStorageQueueMetric(azureMetricName: AzureStorageConstants.Queues.Metrics.TimeSpentInQueue)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, userMessage: "Validation was not successful");
        }
    }
}