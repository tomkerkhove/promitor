using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Newtonsoft.Json;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services
{
    [Category("Integration")]
    public class ResourceDiscoveryTests : IntegrationTest
    {
        private readonly Faker _bogusGenerator = new Faker();

        public ResourceDiscoveryTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
        }

        [Fact]
        public async Task ResourceDiscovery_GetForUnexistingResourceCollection_ReturnsNotFound()
        {
            // Arrange
            string resourceCollectionName = _bogusGenerator.Lorem.Word();
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesAsync(resourceCollectionName);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ResourceDiscovery_GetAllPerResourceTypeWithoutFilters_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceCollectionName = "no-filter";
            const int expectedResourceCount = 8;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesAsync(resourceCollectionName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = JsonConvert.DeserializeObject<List<Resource>>(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnOneResourceGroup_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceCollectionName = "one-resource-group-scenario";
            const int expectedResourceCount = 3;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesAsync(resourceCollectionName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = JsonConvert.DeserializeObject<List<Resource>>(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnTwoResourceGroups_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceCollectionName = "two-resource-group-scenario";
            const int expectedResourceCount = 4;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesAsync(resourceCollectionName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = JsonConvert.DeserializeObject<List<Resource>>(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnOneSubscription_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceCollectionName = "one-subscriptions-scenario";
            const int expectedResourceCount = 2;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesAsync(resourceCollectionName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = JsonConvert.DeserializeObject<List<Resource>>(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnTwoSubscriptions_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceCollectionName = "two-subscriptions-scenario";
            const int expectedResourceCount = 8;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesAsync(resourceCollectionName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = JsonConvert.DeserializeObject<List<Resource>>(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnAppTag_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceCollectionName = "one-tag-scenario";
            const int expectedResourceCount = 3;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesAsync(resourceCollectionName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = JsonConvert.DeserializeObject<List<Resource>>(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnAppAndRegionTag_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceCollectionName = "two-tag-scenario";
            const int expectedResourceCount = 3;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesAsync(resourceCollectionName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = JsonConvert.DeserializeObject<List<Resource>>(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnOneRegion_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceCollectionName = "one-region-scenario";
            const int expectedResourceCount = 1;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesAsync(resourceCollectionName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = JsonConvert.DeserializeObject<List<Resource>>(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscovery_GetWithFilterOnTwoRegions_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceCollectionName = "two-region-scenario";
            const int expectedResourceCount = 7;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesAsync(resourceCollectionName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = JsonConvert.DeserializeObject<List<Resource>>(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }
    }
}
