using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Agents.Scraper.Validation.Steps;
using Promitor.Tests.Unit.Builders.Metrics.v1;
using Promitor.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Tests.Unit.Validation.Scraper.Metrics.ResourceTypes
{
    [Category("Unit")]
    public class EventHubsMetricsDeclarationValidationStepTests : MetricsDeclarationValidationStepsTests
    {
        [Fact]
        public void EventHubsMetricsDeclaration_DeclarationWithoutAzureMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric(azureMetricName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(10001)]
        public void EventHubsMetricsDeclaration_DeclarationWithInvalidMetricLimit_Fails(int metricLimit)
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric(azureMetricLimit: metricLimit)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void EventHubsMetricsDeclaration_UseEntityNameDimensionWithoutTopicName_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric(metricDimensions: new List<string>{"EntityName"}, topicName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void EventHubsMetricsDeclaration_UseEntityNameDimensionWithTopicName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric(metricDimensions: new List<string>{"EntityName"})
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void EventHubsMetricsDeclaration_UseAllowedDimension_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric(metricDimensions: new List<string>{"OperationResult"})
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void EventHubsMetricsDeclaration_DeclarationWithoutResourceAndResourceDiscoveryGroupInfo_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric(omitResource: true)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation was successful but should have failed");
        }

        [Fact]
        public void EventHubsMetricsDeclaration_DeclarationWithoutResourceButWithResourceDiscoveryGroupInfo_Succeeds()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric(omitResource: true, resourceDiscoveryGroupName:"sample-collection")
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void EventHubsMetricsDeclaration_DeclarationWithoutMetricDescription_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric(metricDescription: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void

            EventHubsMetricsDeclaration_DeclarationWithoutMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric(string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void EventHubsMetricsDeclaration_DeclarationWithoutQueueName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric(topicName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }

        [Fact]
        public void EventHubsMetricsDeclaration_DeclarationWithoutServiceBusNamespace_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric(eventHubsNamespace: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationFailed(validationResult);
        }

        [Fact]
        public void EventHubsMetricsDeclaration_ValidDeclaration_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithEventHubsMetric()
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, Mapper);

            // Act
            var metricsDeclarationValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider, NullLogger<MetricsDeclarationValidationStep>.Instance);
            var validationResult = metricsDeclarationValidationStep.Run();

            // Assert
            PromitorAssert.ValidationIsSuccessful(validationResult);
        }
    }
}