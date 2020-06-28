﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GuardNet;
using Microsoft.AspNetCore.Http;
using Promitor.Agents.ResourceDiscovery.Repositories;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Controllers
{
    /// <summary>
    /// API endpoint to discover Azure resources
    /// </summary>
    [ApiController]
    [Route("api/v1/resources/collections")]
    public class DiscoveryController : ControllerBase
    {
        private readonly ResourceRepository _resourceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryController"/> class.
        /// </summary>
        public DiscoveryController(ResourceRepository resourceRepository)
        {
            Guard.NotNull(resourceRepository, nameof(resourceRepository));

            _resourceRepository = resourceRepository;
        }

        /// <summary>
        ///     Discover Resources
        /// </summary>
        /// <remarks>Discovers Azure resources matching the criteria.</remarks>
        [HttpGet("{resourceCollectionName}/discovery", Name = "Discovery_Get")]

        // TODO: Align
        //[SwaggerResponse(200, type: typeof(List<ContainerRegistryResourceDefinition>))]
        //[SwaggerResponse(200, type: typeof(List<ApiManagementResourceDefinition>))]
        [ProducesResponseType(typeof(List<ApiManagementResourceDefinition>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ContainerRegistryResourceDefinition>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string resourceCollectionName)
        {
            var foundResources = await _resourceRepository.GetResourcesAsync(resourceCollectionName);
            if (foundResources == null)
            {
                return NotFound(new { Information = "No resource collection was found with specified name" });
            }

            return Ok(foundResources);
        }
    }
}
