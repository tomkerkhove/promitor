﻿using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class RedisCacheDeserializer : ResourceDeserializer
    {
        private const string CacheNameTag = "cacheName";

        public RedisCacheDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            var cacheName = node.GetString(CacheNameTag);

            return new RedisCacheResourceV1
            {
                CacheName = cacheName
            };
        }
    }
}
