﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Promitor.Agents.Core.Contracts;
using Promitor.Agents.Scraper.Discovery.Interfaces;
using Promitor.Core.Contracts;

namespace Promitor.Agents.Scraper.Discovery
{
    public class StubResourceDiscoveryRepository : IResourceDiscoveryRepository
    {
        private static readonly List<AzureResourceDefinition> EmptyResourceDefinitions = new();
        private static readonly AgentHealthReport HealthReport = new();

        public Task<List<AzureResourceDefinition>> GetResourceDiscoveryGroupAsync(string resourceDiscoveryGroupName)
        {
            return Task.FromResult(EmptyResourceDefinitions);
        }

        public Task<AgentHealthReport> GetHealthAsync()
        {
            return Task.FromResult(HealthReport);
        }
    }
}
