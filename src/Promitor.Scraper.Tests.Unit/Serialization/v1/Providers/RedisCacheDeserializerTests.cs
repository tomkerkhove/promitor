﻿using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Providers;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v1.Providers
{
    [Category("Unit")]
    public class RedisCacheDeserializerTests : ResourceDeserializerTest
    {
        private readonly RedisCacheDeserializer _deserializer;

        public RedisCacheDeserializerTests()
        {
            _deserializer = new RedisCacheDeserializer(NullLogger.Instance);
        }

        [Fact]
        public void Deserialize_CacheNameSupplied_SetsCacheName()
        {
            YamlAssert.PropertySet<RedisCacheResourceV1, AzureResourceDefinitionV1, string>(
                _deserializer,
                "cacheName: promitor-cache",
                "promitor-cache",
                r => r.CacheName);
        }

        [Fact]
        public void Deserialize_CacheNameNotSupplied_Null()
        {
            YamlAssert.PropertyNull<RedisCacheResourceV1, AzureResourceDefinitionV1>(
                _deserializer,
                "resourceGroupName: promitor-group",
                r => r.CacheName);
        }

        protected override IDeserializer<AzureResourceDefinitionV1> CreateDeserializer()
        {
            return new RedisCacheDeserializer(NullLogger.Instance);
        }
    }
}
