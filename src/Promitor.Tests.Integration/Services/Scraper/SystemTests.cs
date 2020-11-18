﻿using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Promitor.Agents.Core;
using Promitor.Agents.Core.Contracts;
using Promitor.Tests.Integration.Clients;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services.Scraper
{
    public class SystemTests : ScraperIntegrationTest
    {
        public SystemTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
        }

        [Fact]
        public async Task System_GetInfo_ReturnsOk()
        {
            // Arrange
            var resourceDiscoveryClient = new ScraperClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetSystemInfoAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawPayload=await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawPayload);
            var systemInfo = JsonConvert.DeserializeObject<SystemInfo>(rawPayload);
            Assert.NotNull(systemInfo);
            Assert.Equal(ExpectedVersion, systemInfo.Version);
            Assert.True(response.Headers.Contains(HttpHeaders.AgentVersion));
            Assert.Equal(ExpectedVersion, response.Headers.GetValues(HttpHeaders.AgentVersion).First());
        }
    }
}
