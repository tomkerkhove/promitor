﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Promitor.Agents.Core;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Tests.Integration.Clients;
using Promitor.Tests.Integration.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.ResourceDiscovery
{
    public class ResourceDiscoveryGroupTests : ResourceDiscoveryIntegrationTest
    {
        public ResourceDiscoveryGroupTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
        }

        [Fact]
        public async Task ResourceDiscoveryGroup_GetAll_ReturnsValidList()
        {
            // Arrange
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetResourceDiscoveryGroupsWithResponseAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resourceDiscoveryGroups = JsonConvert.DeserializeObject<List<ResourceDiscoveryGroup>>(rawResponseBody);
            Assert.NotNull(resourceDiscoveryGroups);
            Assert.NotEmpty(resourceDiscoveryGroups);
        }

        [Fact]
        public async Task ResourceDiscoveryGroup_SuccessfulCall_ReturnsVersionHeader()
        {
            // Arrange
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetResourceDiscoveryGroupsWithResponseAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetFirstOrDefaultHeaderValue(HttpHeaders.AgentVersion));
        }
    }
}
