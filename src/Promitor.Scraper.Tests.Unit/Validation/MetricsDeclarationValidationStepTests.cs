using System.ComponentModel;
using Promitor.Scraper.Tests.Unit.Stubs;
using Promitor.Scraper.Validation.Steps;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation
{
    [Category("Unit")]
    public class MetricsDeclarationValidationStepTests
    {
        private const string ValidMetricDeclaration = @"azureMetadata:
  tenantId: c8819874-9e56-4e3f-b1a8-1c0325138f27
  subscriptionId: 0f9d7fea-99e8-4768-8672-06a28514f77e
  resourceGroupName: promitor
metrics: 
    # v1.0
  - name: demo_queue_size
    description: ""Size of the 'myqueue' queue""
    resourceType: ServiceBusQueue
    namespace: promitor-messaging
    queueName: orders
    azureMetricConfiguration:
      metricName: Size
      aggregation: Average";

        [Fact]
        public void MetricsDeclaration_ValidConfiguration_Succeeds()
        {
            // Arrange
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(ValidMetricDeclaration);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, "Validation was not successful");
        }
    }
}