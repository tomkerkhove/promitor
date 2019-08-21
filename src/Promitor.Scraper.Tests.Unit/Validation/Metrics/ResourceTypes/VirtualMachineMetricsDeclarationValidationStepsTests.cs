using System.ComponentModel;
using AutoMapper;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Mapping;
using Promitor.Scraper.Host.Validation.Steps;
using Promitor.Scraper.Tests.Unit.Builders.Metrics.v1;
using Promitor.Scraper.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Metrics.ResourceTypes
{
    [Category("Unit")]
    public class VirtualMachineMetricsDeclarationValidationStepTests
    {
        private readonly IMapper _mapper;

        public VirtualMachineMetricsDeclarationValidationStepTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<V2MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void VirtualMachineMetricsDeclaration_DeclarationWithoutAzureMetricName_Succeeds()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithVirtualMachineMetric(azureMetricName: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void VirtualMachineMetricsDeclaration_DeclarationWithoutMetricDescription_Succeeded()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithVirtualMachineMetric(metricDescription: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, "Validation was not successful");
        }

        [Fact]
        public void VirtualMachineMetricsDeclaration_DeclarationWithoutMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithVirtualMachineMetric(string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void VirtualMachineMetricsDeclaration_DeclarationWithoutVirtualMachineName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithVirtualMachineMetric(virtualMachineName: string.Empty)
                .Build(_mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, _mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void VirtualMachineMetricsDeclaration_ValidDeclaration_Succeeds()
        {
            // Arrange
            var rawMetricsDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithVirtualMachineMetric()
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