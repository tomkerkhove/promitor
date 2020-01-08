using System.ComponentModel;
using AutoMapper;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Promitor.Scraper.Host.Validation.Steps;
using Promitor.Scraper.Tests.Unit.Builders.Metrics.v1;
using Promitor.Scraper.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Metrics.ResourceTypes
{
    [Category("Unit")]
    public class VirtualMachineScaleSetMetricsDeclarationValidationStepsTests
    {
        private readonly IMapper _mapper;

        public VirtualMachineScaleSetMetricsDeclarationValidationStepsTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void VirtualMachineScaleSetMetricsDeclaration_DeclarationWithoutAzureMetricName_Succeeds()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithVirtualMachineScaleSetMetric(azureMetricName: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void VirtualMachineScaleSetMetricsDeclaration_DeclarationWithoutMetricDescription_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithVirtualMachineScaleSetMetric(metricDescription: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, "Validation was not successful");
        }

        [Fact]
        public void VirtualMachineScaleSetMetricsDeclaration_DeclarationWithoutMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithVirtualMachineScaleSetMetric(string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void VirtualMachineScaleSetMetricsDeclaration_DeclarationWithoutVirtualMachineName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithVirtualMachineScaleSetMetric(scaleSetName: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void VirtualMachineScaleSetMetricsDeclaration_ValidDeclaration_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithVirtualMachineScaleSetMetric()
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawMetricsDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, "Validation was not successful");
        }
    }
}