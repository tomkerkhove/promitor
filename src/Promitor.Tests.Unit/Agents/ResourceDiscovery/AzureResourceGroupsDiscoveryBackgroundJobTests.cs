using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;
using Promitor.Agents.ResourceDiscovery.Scheduling;
using Promitor.Core.Metrics.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Promitor.Tests.Unit.Agents.ResourceDiscovery
{
    public class AzureResourceGroupsDiscoveryBackgroundJobTests
    {
        private readonly Mock<ISystemMetricsPublisher> _systemMetricsPublisher;
        private readonly AzureSubscriptionDiscoveryBackgroundJob _azureResourceGroupsDiscoveryBackgroundJob;
        private readonly Promitor.Core.Contracts.PagedPayload<AzureSubscriptionInformation> _firstPage;

        public AzureResourceGroupsDiscoveryBackgroundJobTests()
        {
            var azureResourceRepository = new Mock<IAzureResourceRepository>(MockBehavior.Strict);
            _systemMetricsPublisher = new Mock<ISystemMetricsPublisher>(MockBehavior.Strict);
            var logger = new Mock<ILogger<AzureSubscriptionDiscoveryBackgroundJob>>(MockBehavior.Loose);
            _azureResourceGroupsDiscoveryBackgroundJob = new AzureSubscriptionDiscoveryBackgroundJob("jobName", azureResourceRepository.Object, _systemMetricsPublisher.Object, logger.Object);

            _firstPage = new Promitor.Core.Contracts.PagedPayload<AzureSubscriptionInformation>()
            {
                PageInformation = new Promitor.Core.Contracts.PageInformation { CurrentPage = 1, PageSize = 1, TotalRecords = 1 },
                Result = new List<AzureSubscriptionInformation>() { new() { TenantId = "TenantId", Id = "ID", Name = "Name" } }
            };
            var secondPage = new Promitor.Core.Contracts.PagedPayload<AzureSubscriptionInformation>()
            {
                PageInformation = new Promitor.Core.Contracts.PageInformation { CurrentPage = 2, PageSize = 1, TotalRecords = 1 },
                Result = new List<AzureSubscriptionInformation>() { new() { TenantId = "TenantId", Id = "ID", Name = "Name" } }
            };
            azureResourceRepository.Setup(r => r.DiscoverAzureSubscriptionsAsync(1000, 1)).ReturnsAsync(_firstPage);
            azureResourceRepository.Setup(r => r.DiscoverAzureSubscriptionsAsync(1000, 2)).ReturnsAsync(secondPage);
            _systemMetricsPublisher.Setup(p => p.WriteGaugeMeasurementAsync(It.IsAny<string>(), It.IsAny<string>(), 1, It.IsAny<Dictionary<string, string>>(), true)).Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task AzureResourceGroupsDiscoveryBackgroundJob_Execution_Succeeds()
        {
            // Act
            await _azureResourceGroupsDiscoveryBackgroundJob.ExecuteAsync(CancellationToken.None);

            // Assert
            _systemMetricsPublisher.Verify(p => p.WriteGaugeMeasurementAsync("promitor_azure_landscape_subscription_info", It.IsAny<string>(), 1,
                It.Is<Dictionary<string, string>>(d =>
                d["tenant_id"] == "TenantId" &&
                d["subscription_id"] == "ID" &&
                d["subscription_name"] == "Name"
                )
                , true), Times.Once);
        }

        [Fact]
        public async Task AzureResourceGroupsDiscoveryBackgroundJob_Execution_LoopsOnMultiplePages()
        {
            // Arrange
            _firstPage.PageInformation.TotalRecords = 2;

            // Act
            await _azureResourceGroupsDiscoveryBackgroundJob.ExecuteAsync(CancellationToken.None);

            // Assert
            _systemMetricsPublisher.Verify(p => p.WriteGaugeMeasurementAsync("promitor_azure_landscape_subscription_info", It.IsAny<string>(), 1,
                It.Is<Dictionary<string, string>>(d =>
                d["tenant_id"] == "TenantId" &&
                d["subscription_id"] == "ID" &&
                d["subscription_name"] == "Name"
                )
                , true), Times.Exactly(2));
        }
    }
}