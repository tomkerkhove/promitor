using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace Promitor.Tests.Integration.Services
{
    [Category("Integration")]
    public class ResourceCollectionTests : IntegrationTest
    {
        public ResourceCollectionTests(ITestOutputHelper testOutput)
          : base(testOutput)
        {
        }

        [Fact]
        public async Task ResourceCollection_GetAll_ReturnsValidList()
        {
            // Arrange
            var resourceDiscoveryClient = new ResourceDiscoveryClient(Configuration, Logger);

            // Act
            var response = await resourceDiscoveryClient.GetResourceCollectionsAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var rawResponseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(rawResponseBody);
            var resourceCollections = JsonConvert.DeserializeObject<List<ResourceCollection>>(rawResponseBody);
            Assert.NotNull(resourceCollections);
            Assert.NotEmpty(resourceCollections);
        }
    }
}
