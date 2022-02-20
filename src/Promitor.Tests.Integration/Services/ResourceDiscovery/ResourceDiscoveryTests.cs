using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Logging;
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

        [Theory]
        [InlineData("v1")]
        [InlineData("v2")]
        public async Task ResourceDiscovery_GetForUnexistingResourceDiscoveryGroup_ReturnsNotFound(string apiVersion)
        {
            // Arrange
            string resourceDiscoveryGroupName = _bogusGenerator.Lorem.Word();
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(apiVersion, resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetFirstOrDefaultHeaderValue(HttpHeaders.AgentVersion));
        }

        [Theory]
        [InlineData("v1")]
        [InlineData("v2")]
        public async Task ResourceDiscovery_SuccessfulCall_ReturnsVersionHeader(string apiVersion)
        {
            // Arrange
            const string resourceDiscoveryGroupName = "logic-apps-unfiltered";
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(apiVersion, resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetFirstOrDefaultHeaderValue(HttpHeaders.AgentVersion));
        }

        [Theory]
        [InlineData("v1", -1)]
        [InlineData("v1", 0)]
        [InlineData("v1", 1001)]
        [InlineData("v2", -1)]
        [InlineData("v2", 0)]
        [InlineData("v2", 1001)]
        public async Task ResourceDiscovery_SuccessfulCallWithInvalidPageSize_ReturnsBadRequest(string apiVersion, int pageSize)
        {
            // Arrange
            const string resourceDiscoveryGroupName = "logic-apps-unfiltered";
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(apiVersion, resourceDiscoveryGroupName, pageSize: pageSize);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("v1")]
        [InlineData("v2")]
        public async Task ResourceDiscovery_SuccessfulCallWithInvalidCurrentPage_ReturnsBadRequest(string apiVersion)
        {
            // Arrange
            const string resourceDiscoveryGroupName = "logic-apps-unfiltered";
            const int currentPage = 0;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesWithResponseAsync(apiVersion, resourceDiscoveryGroupName, currentPage);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ResourceDiscoveryV2_GetAllPerResourceTypeWithoutFilters_ReturnsExpectedAmountThroughPaging()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "logic-apps-unfiltered";
            const int pageSize = 1000;
            const int expectedTotalResourceCount = 1018;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act & Assert
            var currentPage = 1;
            bool hasMore;
            do
            {
                Logger.LogInformation("Discovering resources with page size for page {CurrentPage}", currentPage);
                
                var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName, currentPage: currentPage);
                var resources = await AssertAndGetPagedResult(response);
                Assert.NotNull(resources);
                
                var delta = expectedTotalResourceCount - ((currentPage - 1) * pageSize);
                var amountOfExpectedResults = delta > pageSize ? pageSize : delta;
                Assert.Equal(amountOfExpectedResults, resources.Result.Count);
                Assert.Equal(expectedTotalResourceCount, resources.TotalRecords);

                hasMore = resources.HasMore;
                currentPage++;
            }
            while (hasMore);
        }

        [Fact]
        public async Task ResourceDiscoveryV2_GetAllPerResourceTypeWithoutFiltersAndSpecificPageSize_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "logic-apps-unfiltered";
            const int expectedResourceCount = 1018;
            const int pageSize = 500;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName, pageSize: pageSize);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(pageSize, resources.Result.Count);
            Assert.Equal(expectedResourceCount, resources.TotalRecords);
        }

        [Fact]
        public async Task ResourceDiscoveryV2_GetWithFilterOnOneResourceGroup_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-resource-group-scenario";
            const int expectedResourceCount = 6;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV2_GetWithFilterOnOneResourceGroupWithDifferentCasing_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-resource-group-scenario-with-other-casing";
            const int expectedResourceCount = 6;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV2_GetWithFilterOnTwoResourceGroups_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-resource-group-scenario";
            const int expectedResourceCount = 9;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV2_GetWithFilterOnOneSubscription_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-subscriptions-scenario";
            const int expectedResourceCount = 1000;  // This should be more but is a known bug, see https://github.com/tomkerkhove/promitor/issues/1828
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact (Skip = "We only have one Azure test subscription")]
        public async Task ResourceDiscoveryV2_GetWithFilterOnTwoSubscriptions_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-subscriptions-scenario";
            const int expectedResourceCount = 11;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV2_GetWithFilterOnAppTag_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-tag-scenario";
            const int expectedResourceCount = 9;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV2_GetWithFilterOnAppAndRegionTag_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-tag-scenario";
            const int expectedResourceCount = 409;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV2_GetWithFilterOnMultipleValueInstanceTag_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-instance-tag-with-multi-value-scenario";
            const int expectedResourceCount = 409;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV2_GetWithFilterOnOneRegion_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-region-scenario";
            const int expectedResourceCount = 203;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV2_GetWithFilterOnTwoRegions_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-region-scenario";
            const int expectedResourceCount = 415;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV2WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Result.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV1_GetAllPerResourceTypeWithoutFilters_ReturnsExpectedAmountThroughPagingInHeaders()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "logic-apps-unfiltered";
            const int pageSize = 1000;
            const int expectedTotalResourceCount = 1018;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act & Assert
            var currentPage = 1;
            bool hasMore;
            do
            {
                Logger.LogInformation("Discovering resources with page size for page {CurrentPage}", currentPage);

                var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName, currentPage: currentPage);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var rawResponseBody = await response.Content.ReadAsStringAsync();
                Assert.NotEmpty(rawResponseBody);
                var resources = DeserializeRawV1Response(rawResponseBody);
                Assert.NotNull(resources);

                var delta = expectedTotalResourceCount - ((currentPage - 1) * pageSize);
                var amountOfExpectedResults = delta > pageSize ? pageSize : delta;
                Assert.Equal(amountOfExpectedResults, resources.Count);
                var totalRecordsInResponse = AssertAndGetHeaderValue("X-Paging-Total", response);
                Assert.Equal(expectedTotalResourceCount, totalRecordsInResponse);

                hasMore = resources.Count == pageSize;
                currentPage++;
            }
            while (hasMore);
        }

        [Fact]
        public async Task ResourceDiscoveryV1_GetAllPerResourceTypeWithoutFiltersAndSpecificPageSize_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "logic-apps-unfiltered";
            const int expectedResourceCount = 1018;
            const int pageSize = 500;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName, pageSize: pageSize);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV1Response(rawResponseBody);
            Assert.NotNull(resources);
            var pageSizeInResponse = AssertAndGetHeaderValue("X-Paging-Page-Size", response);
            var totalRecordsInResponse = AssertAndGetHeaderValue("X-Paging-Total", response);
            Assert.Equal(pageSize, pageSizeInResponse);
            Assert.Equal(expectedResourceCount, totalRecordsInResponse);
        }

        [Fact]
        public async Task ResourceDiscoveryV1_GetWithFilterOnOneResourceGroup_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-resource-group-scenario";
            const int expectedResourceCount = 6;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV1Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV1_GetWithFilterOnOneResourceGroupWithDifferentCasing_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-resource-group-scenario-with-other-casing";
            const int expectedResourceCount = 6;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV1Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV1_GetWithFilterOnTwoResourceGroups_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-resource-group-scenario";
            const int expectedResourceCount = 9;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV1Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV1_GetWithFilterOnOneSubscription_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-subscriptions-scenario";
            const int expectedResourceCount = 1000;  // This should be more but is a known bug, see https://github.com/tomkerkhove/promitor/issues/1828
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV1Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact(Skip = "We only have one Azure test subscription")]
        public async Task ResourceDiscoveryV1_GetWithFilterOnTwoSubscriptions_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-subscriptions-scenario";
            const int expectedResourceCount = 11;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV1Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV1_GetWithFilterOnAppTag_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-tag-scenario";
            const int expectedResourceCount = 9;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV1Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV1_GetWithFilterOnAppAndRegionTag_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-tag-scenario";
            const int expectedResourceCount = 409;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV1Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV1_GetWithFilterOnMultipleValueInstanceTag_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-instance-tag-with-multi-value-scenario";
            const int expectedResourceCount = 409;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV1Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV1_GetWithFilterOnOneRegion_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "one-region-scenario";
            const int expectedResourceCount = 203;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV1Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        [Fact]
        public async Task ResourceDiscoveryV1_GetWithFilterOnTwoRegions_ReturnsExpectedAmount()
        {
            // Arrange
            const string resourceDiscoveryGroupName = "two-region-scenario";
            const int expectedResourceCount = 415;
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetDiscoveredResourcesV1WithResponseAsync(resourceDiscoveryGroupName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV1Response(rawResponseBody);
            Assert.NotNull(resources);
            Assert.Equal(expectedResourceCount, resources.Count);
        }

        private static int AssertAndGetHeaderValue(string headerName, HttpResponseMessage response)
        {
            var hasHeader = response.Headers.Contains(headerName);
            Assert.True(hasHeader);
            var rawPageSizeInResponse = response.Headers.GetValues(headerName).First();
            var pageSizeInResponse = int.Parse(rawPageSizeInResponse);
            return pageSizeInResponse;
        }

        private async Task<PagedResult<List<Resource>>> AssertAndGetPagedResult(HttpResponseMessage response)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resources = DeserializeRawV2Response(rawResponseBody);
            return resources;
        }

        private List<Resource> DeserializeRawV1Response(string rawResponseBody)
        {
            return JsonConvert.DeserializeObject<List<Resource>>(rawResponseBody);
        }

        private PagedResult<List<Resource>> DeserializeRawV2Response(string rawResponseBody)
        {
            return JsonConvert.DeserializeObject<PagedResult<List<Resource>>>(rawResponseBody);
        }
    }
}
