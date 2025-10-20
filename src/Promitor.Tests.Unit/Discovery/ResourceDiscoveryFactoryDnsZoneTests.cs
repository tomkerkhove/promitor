using System.ComponentModel;
using Promitor.Agents.ResourceDiscovery.Graph;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Promitor.Core.Contracts;
using Xunit;

namespace Promitor.Tests.Unit.Discovery
{
    [Category("Unit")]
    public class ResourceDiscoveryFactoryDnsZoneTests : UnitTest
    {
        [Fact]
        public void UseResourceDiscoveryFor_DnsZone_ReturnsDnsZoneDiscoveryQuery()
        {
            // Act
            var result = ResourceDiscoveryFactory.UseResourceDiscoveryFor(ResourceType.DnsZone);

            // Assert
            Assert.IsType<DnsZoneDiscoveryQuery>(result);
        }
    }
}