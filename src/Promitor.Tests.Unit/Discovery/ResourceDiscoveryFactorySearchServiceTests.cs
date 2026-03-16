using System.ComponentModel;
using Promitor.Agents.ResourceDiscovery.Graph;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Promitor.Core.Contracts;
using Xunit;

namespace Promitor.Tests.Unit.Discovery
{
    [Category("Unit")]
    public class ResourceDiscoveryFactorySearchServiceTests : UnitTest
    {
        [Fact]
        public void UseResourceDiscoveryFor_SearchService_ReturnsSearchServiceDiscoveryQuery()
        {
            // Act
            var result = ResourceDiscoveryFactory.UseResourceDiscoveryFor(ResourceType.SearchService);

            // Assert
            Assert.IsType<SearchServiceDiscoveryQuery>(result);
        }
    }
}
