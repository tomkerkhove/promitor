using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Graph.Interfaces;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Promitor.Tests.Unit.Agents.ResourceDiscovery
{
    public class AzureResourceRepositoryTests
    {
        private const string basePath = @"Agents\ResourceDiscovery\TestData\";

        private readonly Mock<IAzureResourceGraph> azureResourceGraph;
        private readonly Mock<IOptionsMonitor<ResourceDeclaration>> resourceDeclarationMonitor;
        private readonly Mock<ILogger<AzureResourceRepository>> logger;

        private readonly AzureResourceRepository azureResourceRepository;

        public AzureResourceRepositoryTests()
        {
            azureResourceGraph = new Mock<IAzureResourceGraph>(MockBehavior.Strict);
            resourceDeclarationMonitor = new Mock<IOptionsMonitor<ResourceDeclaration>>(MockBehavior.Strict);
            logger = new Mock<ILogger<AzureResourceRepository>>(MockBehavior.Strict);

            azureResourceRepository = new AzureResourceRepository(azureResourceGraph.Object, resourceDeclarationMonitor.Object, logger.Object);
        }

        [Fact]
        public async Task AzureSubscriptions_Discovery_Succeeds()
        {
            var testData = ReadTestData("DiscoverAzureSubscriptionsAsync.json");
            azureResourceGraph.Setup(m => m.QueryAzureLandscapeAsync("Discover Azure Subscriptions", It.IsAny<string>())).ReturnsAsync(testData);
            var expectedSubscriptions = GetAzureSubscriptions();

            var azureSubscriptions = await azureResourceRepository.DiscoverAzureSubscriptionsAsync();

            Assert.NotStrictEqual(expectedSubscriptions, azureSubscriptions);

            azureResourceGraph.Verify(m => m.QueryAzureLandscapeAsync("Discover Azure Subscriptions", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AzureResourceGroups_Discovery_Succeeds()
        {
            var testData = ReadTestData("DiscoverAzureResourceGroupsAsync.json");
            azureResourceGraph.Setup(m => m.QueryAzureLandscapeAsync("Discover Azure Resource Groups", It.IsAny<string>())).ReturnsAsync(testData);
            var expectedResourceGroups = GetAzureResourceGroups();

            var azureResourceGroups = await azureResourceRepository.DiscoverAzureResourceGroupsAsync();

            Assert.NotStrictEqual(expectedResourceGroups, azureResourceGroups);

            azureResourceGraph.Verify(m => m.QueryAzureLandscapeAsync("Discover Azure Resource Groups", It.IsAny<string>()), Times.Once);
        }

        private List<AzureSubscriptionInformation> GetAzureSubscriptions()
        {
            return new List<AzureSubscriptionInformation> {
                 GetAzureSubscriptionInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "Visual Studio Enterprise",
                                                 "Enabled",
                                                 "Off",
                                                 "Sponsored_2016-01-01",
                                                 "RoleBased"),

                 GetAzureSubscriptionInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0329dd2a-59dc-4493-aa54-cb01cb027dc2",
                                                 "Windows Azure MSDN - Visual Studio Ultimate",
                                                 "Enabled",
                                                 "On",
                                                 "MSDN_2014-09-01",
                                                 "RoleBased")
            };
        }

        private AzureSubscriptionInformation GetAzureSubscriptionInformation(string tenantId, string id, string name, string state, string spendingLimit, string quotaId, string authorizationSource)
        {
            return new AzureSubscriptionInformation
            {
                Id = id,
                TenantId = tenantId,
                Name = name,
                AuthorizationSource = authorizationSource,
                QuotaId = quotaId,
                SpendingLimit = spendingLimit,
                State = state
            };
        }

        private List<AzureResourceGroupInformation> GetAzureResourceGroups()
        {
            return new List<AzureResourceGroupInformation>
            {
                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0329dd2a-59dc-4493-aa54-cb01cb027dc2",
                                                 "cds74642ORBE57UVKYYBQ255LLUAMD7R7KEFPG2YR5SNYAO7THHKAUB5BBFQ-North-Central-US",
                                                 "northcentralus",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0329dd2a-59dc-4493-aa54-cb01cb027dc2",
                                                 "promitor-sources",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                  "0329dd2a-59dc-4493-aa54-cb01cb027dc2",
                                                  "Default-Storage-WestEurope",
                                                  "westeurope",
                                                  "Succeeded",
                                                  ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                  "0329dd2a-59dc-4493-aa54-cb01cb027dc2",
                                                  "promitor-testing-resource-discovery-us",
                                                  "westus2",
                                                  "Succeeded",
                                                  ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "azure-deprecation",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "promitor-sandbox",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "demo-central-us",
                                                 "centralus",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "cloud-shell-storage-eastus",
                                                 "eastus",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "promitor",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "NetworkWatcherRG",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "promitor-sources",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "keda-demos",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "promitor-testing-resource-discovery-eu",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "DefaultResourceGroup-WEU",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "demo",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "azure-monitor-autoscale-to-event-grid-adapter",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "MC_keda-demos_keda-demos_westeurope",
                                                 "westeurope",
                                                 "Succeeded",
                                                 "/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourcegroups/keda-demos/providers/Microsoft.ContainerService/managedClusters/keda-demos"),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "aks-virtual-node",
                                                 "westeurope",
                                                 "Succeeded",
                                                 ""),

                GetAzureResourceGroupInformation("c8819874-9e56-4e3f-b1a8-1c0325138f27",
                                                 "0f9d7fea-99e8-4768-8672-06a28514f77e",
                                                 "virtual-node-preview",
                                                 "westeurope",
                                                 "Succeeded",
                                                 "")
            };
        }

        private AzureResourceGroupInformation GetAzureResourceGroupInformation(string tenantId, string subscriptionId, string name, string region, string provisioningState, string managedBy)
        {
            return new AzureResourceGroupInformation
            {
                TenantId = tenantId,
                Name = name,
                SubscriptionId = subscriptionId,
                ManagedBy = managedBy,
                Region = region,
                ProvisioningState = provisioningState
            };
        }

        private JObject ReadTestData(string fileName)
        {
            var fileContents = File.ReadAllText(basePath + fileName);
            return JObject.Parse(fileContents);
        }
    }
}
