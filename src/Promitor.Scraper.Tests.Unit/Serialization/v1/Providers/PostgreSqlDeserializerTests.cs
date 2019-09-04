﻿using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    public class PostgreSqlDeserializerTests : ResourceDeserializerTest
    {
        private readonly PostgreSqlDeserializer _deserializer;

        public PostgreSqlDeserializerTests()
        {
            _deserializer = new PostgreSqlDeserializer(NullLogger.Instance);
        }

        [Fact]
        public void Deserialize_ServerNameSupplied_SetsServerName()
        {
            YamlAssert.PropertySet<PostgreSqlResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "serverName: promitor-db",
                "promitor-db",
                r => r.ServerName);
        }

        [Fact]
        public void Deserialize_ServerNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<PostgreSqlResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.ServerName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new PostgreSqlDeserializer(NullLogger.Instance);
        }
    }
}
