using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Newtonsoft.Json;
using Promitor.Agents.Core;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Core.Contracts;
using Promitor.Tests.Integration.Clients;
using Promitor.Tests.Integration.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.ResourceDiscovery
{
    public class ResourceDiscoveryTests : ResourceDiscoveryIntegrationTest
    {
        private readonly Faker _bogusGenerator = new Faker();

        public ResourceDiscoveryTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
        }

        [Fact]
        public async Task ResourceDiscovery_GetForUnexistingResourceDiscoveryGroup_ReturnsNotFound()
        {
            // Arrange
            string resourceDiscoveryGroupName = _bogusGenerator.Lorem.Word();
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetFirstOrDefaultHeaderValue(HttpHeaders.AgentVersion));
        }

        [Fact]
        public async Task ResourceDiscovery_SuccessfulCall_ReturnsVersionHeader()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "logic-apps-unfiltered";
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetFirstOrDefaultHeaderValue(HttpHeaders.AgentVersion));
        }

        [Fact]
        public async Task ResourceDiscovery_GetAllPerResourceTypeWithoutFilters_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "logic-apps-unfiltered";
            const int expectedResourceCount = 1000; // This should be more but is a known bug, see https://github.com/tomkerkhove/promitor/issues/1828
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawResponse(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnOneResourceGroup_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-resource-group-scenario";
            const int expectedResourceCount = 6;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawResponse(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnOneResourceGroupWithDifferentCasing_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-resource-group-scenario-with-other-casing";
            const int expectedResourceCount = 6;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawResponse(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnTwoResourceGroups_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-resource-group-scenario";
            const int expectedResourceCount = 9;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawResponse(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnOneSubscription_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-subscriptions-scenario";
            const int expectedResourceCount = 1000;  // This should be more but is a known bug, see https://github.com/tomkerkhove/promitor/issues/1828
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawResponse(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact (Skip = "We only have one Azure test subscription")]
        public async Task ResourceDiscovery_GetWithFilterOnTwoSubscriptions_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-subscriptions-scenario";
            const int expectedResourceCount = 11;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawResponse(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnAppTag_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-tag-scenario";
            const int expectedResourceCount = 9;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawResponse(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnAppAndRegionTag_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-tag-scenario";
            const int expectedResourceCount = 409;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawResponse(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnMultipleValueInstanceTag_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-instance-tag-with-multi-value-scenario";
            const int expectedResourceCount = 409;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawResponse(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnOneRegion_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-region-scenario";
            const int expectedResourceCount = 203;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawResponse(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnTwoRegions_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-region-scenario";
            const int expectedResourceCount = 415;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawResponse(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        private PagedResult<List<Resource>> DeserializeRawResponse(string rawResponseBody)
        {
            return JsonConvert.DeserializeObject<PagedResult<List<Resource>>>(rawResponseBody);
        }
    }
}
