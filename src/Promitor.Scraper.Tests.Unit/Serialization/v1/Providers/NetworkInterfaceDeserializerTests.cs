﻿using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
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
            DeserializerTestHelpers.AssertPropertySet<NetworkInterfaceResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "networkInterfaceName: promitor-nic",
                "promitor-nic",
                r => r.NetworkInterfaceName);
        }

        [Fact]
        public void Deserialize_NetworkInterfaceNameNotSupplied_Null()
        {
            DeserializerTestHelpers.AssertPropertyNull<NetworkInterfaceResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.NetworkInterfaceName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new NetworkInterfaceDeserializer(new Mock<ILogger>().Object);
        }
    }
}
