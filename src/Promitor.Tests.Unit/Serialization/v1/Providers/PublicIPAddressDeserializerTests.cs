﻿using System.ComponentModel;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class PublicIPAddressDeserializerTests : ResourceDeserializerTest<PublicIPAddressDeserializer>
    {
        private readonly PublicIPAddressDeserializer _deserializer;

        public PublicIPAddressDeserializerTests()
        {
            _deserializer = new PublicIPAddressDeserializer(Logger);
        }

        [Fact]
        public void Deserialize_PublicIPAddressNameSupplied_SetsName()
        {
            YamlAssert.PropertySet<PublicIPAddressResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "publicIPAddressName: promitor-ip-address",
                "promitor-ip-address",
                r => r.PublicIPAddressName);
        }

        [Fact]
        public void Deserialize_PublicIPAddressNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<PublicIPAddressResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.PublicIPAddressName);
        }

        [Fact]
        public void Deserialize_PublicIPAddressNameNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("resourceGroupName: promitor-group");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "publicIPAddressName");
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new PublicIPAddressDeserializer(Logger);
        }
    }
}
