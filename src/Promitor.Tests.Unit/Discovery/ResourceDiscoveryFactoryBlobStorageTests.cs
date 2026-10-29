using System.ComponentModel;
using Promitor.Agents.ResourceDiscovery.Graph;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Promitor.Core.Contracts;
using Xunit;

namespace Promitor.Tests.Unit.Discovery
{
    [Category("Unit")]
    public class ResourceDiscoveryFactoryBlobStorageTests : UnitTest
    {
        [Fact]
        public void UseResourceDiscoveryFor_BlobStorage_ReturnsBlobStorageDiscoveryQuery()
        {
            // Act
            var result = ResourceDiscoveryFactory.UseResourceDiscoveryFor(ResourceType.BlobStorage);

            // Assert
            Assert.IsType<BlobStorageDiscoveryQuery>(result);
        }
    }
}
