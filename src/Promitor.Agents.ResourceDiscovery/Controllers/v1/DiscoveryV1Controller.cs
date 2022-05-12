using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Promitor.Agents.ResourceDiscovery.Graph.Exceptions;
using Promitor.Agents.ResourceDiscovery.Graph.Repositories.Interfaces;
using Promitor.Core.Contracts;

namespace Promitor.Agents.ResourceDiscovery.Controllers.v1
{
    /// <summary>
    /// API endpoint to discover Azure resources
    /// </summary>
    [ApiController]
    [Route("api/v1/resources/groups")]
    public class DiscoveryV1Controller : ControllerBase
    {
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IAzureResourceRepository _azureResourceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryV1Controller"/> class.
        /// </summary>
        public DiscoveryV1Controller(IAzureResourceRepository azureResourceRepository)
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
        [ProducesResponseType(typeof(List<AzureResourceDefinition>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string resourceDiscoveryGroup, int pageSize = 1000, int currentPage = 1)
        {
            if (currentPage < 1) 
            {
                return ValidationProblem(detail: "Current page has to be 1 or more");
            }

            if (pageSize < 1)
            {
                return ValidationProblem(detail: "Page size cannot be less than 1");
            }

            if (pageSize > 1000)
            {
                return ValidationProblem(detail: "Page size cannot be higher than 1000");
            }

            try
            {
                var pagedDiscoveredResources = await _azureResourceRepository.GetResourcesAsync(resourceDiscoveryGroup, pageSize, currentPage);
                if (pagedDiscoveredResources == null)
                {
                    return NotFound(new {Information = "No resource discovery group was found with specified name"});
                }

                Response.Headers.Add("X-Paging-Page-Size", pagedDiscoveredResources.PageInformation.PageSize.ToString());
                Response.Headers.Add("X-Paging-Current-Page", pagedDiscoveredResources.PageInformation.CurrentPage.ToString());
                Response.Headers.Add("X-Paging-Total", pagedDiscoveredResources.PageInformation.TotalRecords.ToString());

                var serializedResources = JsonConvert.SerializeObject(pagedDiscoveredResources.Result, _serializerSettings);

                var response = Content(serializedResources, "application/json");
                response.StatusCode = (int) HttpStatusCode.OK;
                
                return response;
            }
            catch (ResourceTypeNotSupportedException resourceTypeNotSupportedException)
            {
                return StatusCode((int) HttpStatusCode.NotImplemented, new ResourceDiscoveryFailedDetails {Details = $"Resource type '{resourceTypeNotSupportedException.ResourceType}' for discovery group '{resourceDiscoveryGroup}' is not supported"});
            }
        }
    }
}
