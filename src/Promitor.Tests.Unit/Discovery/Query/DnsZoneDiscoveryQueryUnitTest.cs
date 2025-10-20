using System;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes;
using Promitor.Core.Contracts.ResourceTypes;
using Xunit;

namespace Promitor.Tests.Unit.Discovery.Query
{
    [Category("Unit")]
    public class DnsZoneDiscoveryQueryUnitTest : UnitTest
    {
        [Fact]
        public void ParseResults_ValidRow_Succeeds()
        {
            // Arrange
            var subscriptionId = Guid.NewGuid().ToString();
            var resourceGroup = "dns-rg";
            var zoneName = "example.com";
            var row = JArray.FromObject(new object[]
            {
                subscriptionId,
                resourceGroup,
                "microsoft.network/dnszones",
                zoneName
            });
            var query = new DnsZoneDiscoveryQuery();

            // Act
            var result = query.ParseResults(row);

            // Assert
            var dnsDef = Assert.IsType<DnsZoneResourceDefinition>(result);
            Assert.Equal(subscriptionId, dnsDef.SubscriptionId);
            Assert.Equal(resourceGroup, dnsDef.ResourceGroupName);
            Assert.Equal(zoneName, dnsDef.ZoneName);
            Assert.Equal(zoneName, dnsDef.ResourceName);
        }

        [Fact]
        public void ParseResults_NullRow_Throws()
        {
            // Arrange
            var query = new DnsZoneDiscoveryQuery();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => query.ParseResults(null));
        }
    }
}
