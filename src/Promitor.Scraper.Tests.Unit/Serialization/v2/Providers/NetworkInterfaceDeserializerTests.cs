using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Providers
{
    [Category("Unit")]
    public class NetworkInterfaceDeserializerTests : ResourceDeserializerTestBase
    {
        private readonly NetworkInterfaceDeserializer _deserializer;

        public NetworkInterfaceDeserializerTests()
        {
            _deserializer = new NetworkInterfaceDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_NetworkInterfaceNameSupplied_SetsNetworkInterfaceName()
        {
            DeserializerTestHelpers.AssertPropertySet<NetworkInterfaceResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "networkInterfaceName: promitor-nic",
                "promitor-nic",
                r => r.NetworkInterfaceName);
        }

        [Fact]
        public void Deserialize_NetworkInterfaceNameNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<NetworkInterfaceResourceV2, AzureResourceDefinitionV2, string>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.NetworkInterfaceName);
        }

        protected override IDeserializer<AzureResourceDefinitionV2> CreateDeserializer()
        {
            return new NetworkInterfaceDeserializer(new Mock<ILogger>().Object);
        }
    }
}
