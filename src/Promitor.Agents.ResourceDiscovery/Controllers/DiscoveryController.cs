using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GuardNet;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Promitor.Agents.ResourceDiscovery.Graph.Exceptions;
using Promitor.Core.Contracts;
using Newtonsoft.Json.Converters;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;

namespace Promitor.Agents.ResourceDiscovery.Controllers
{
    /// <summary>
    /// API endpoint to discover Azure resources
    /// </summary>
    [ApiController]
    [Route("api/v1/resources/groups")]
    public class DiscoveryController : ControllerBase
    {
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IAzureResourceRepository _azureResourceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryController"/> class.
        /// </summary>
        public DiscoveryController(IAzureResourceRepository azureResourceRepository)
        {
            Guard.NotNull(azureResourceRepository, nameof(azureResourceRepository));

            _azureResourceRepository = azureResourceRepository;
            _serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        /// <summary>
        ///     Discover Resources
        /// </summary>
        /// <remarks>Discovers Azure resources matching the criteria.</remarks>
        [HttpGet("{resourceDiscoveryGroup}/discover", Name = "Discovery_Get")]
        [ProducesResponseType(typeof(PagedResult<List<AzureResourceDefinition>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string resourceDiscoveryGroup, int pageSize = 1000, int currentPage = 1)
        {
            if (currentPage < 1) 
            {
                // TODO: Integration test for this
                return ValidationProblem(detail: "Current page has to be 1 or more");
            }

            try
            {
                var pagedDiscoveredResources = await _azureResourceRepository.GetResourcesAsync(resourceDiscoveryGroup, pageSize, currentPage);
                if (pagedDiscoveredResources == null)
                {
                    return NotFound(new {Information = "No resource discovery group was found with specified name"});
                }

                var serializedResources = JsonConvert.SerializeObject(pagedDiscoveredResources, _serializerSettings);

                var response = Content(serializedResources, "application/json");
                response.StatusCode = (int) HttpStatusCode.OK;

                // TODO: Add paging

                return response;
            }
            catch (ResourceTypeNotSupportedException resourceTypeNotSupportedException)
            {
                return StatusCode((int) HttpStatusCode.NotImplemented, new ResourceDiscoveryFailedDetails {Details = $"Resource type '{resourceTypeNotSupportedException.ResourceType}' for discovery group '{resourceDiscoveryGroup}' is not supported"});
            }
        }
    }
}
