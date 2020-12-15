﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        Task<List<AzureResourceDefinition>> GetResourcesAsync(string resourceDiscoveryGroupName);
    }
}
