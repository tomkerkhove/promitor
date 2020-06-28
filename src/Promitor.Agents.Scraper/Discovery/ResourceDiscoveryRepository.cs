﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GuardNet;
using Promitor.Core.Contracts;

namespace Promitor.Agents.Scraper.Discovery
{
    public class ResourceDiscoveryRepository
    {
        private readonly ResourceDiscoveryClient _resourceDiscoveryClient;

        public ResourceDiscoveryRepository(ResourceDiscoveryClient resourceDiscoveryClient)
        {
            Guard.NotNull(resourceDiscoveryClient, nameof(resourceDiscoveryClient));

            _resourceDiscoveryClient = resourceDiscoveryClient;
        }

        public async Task<List<AzureResourceDefinition>> GetResourceCollectionAsync(string resourceCollectionName)
        {
            Guard.NotNullOrWhitespace(resourceCollectionName,nameof(resourceCollectionName));

            var resources = await _resourceDiscoveryClient.GetAsync(resourceCollectionName);
            return resources;
        }
    }
}