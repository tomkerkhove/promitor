using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GuardNet;
using Newtonsoft.Json;
using Promitor.Agents.ResourceDiscovery.Graph.Exceptions;
using Promitor.Agents.ResourceDiscovery.Repositories;
using Promitor.Core.Contracts;
using Newtonsoft.Json.Converters;

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
        private readonly ResourceRepository _resourceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryController"/> class.
        /// </summary>
        public DiscoveryController(ResourceRepository resourceRepository)
        {
            Guard.NotNull(resourceRepository, nameof(resourceRepository));

            _resourceRepository = resourceRepository;
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
        public async Task<IActionResult> Get(string resourceDiscoveryGroup)
        {
            try
            {
                var foundResources = await _resourceRepository.GetResourcesAsync(resourceDiscoveryGroup);
                if (foundResources == null)
                {
                    return NotFound(new {Information = "No resource discovery group was found with specified name"});
                }

                var serializedResources = JsonConvert.SerializeObject(foundResources, _serializerSettings);
                
                var response= Content(serializedResources, "application/json");
                response.StatusCode = (int) HttpStatusCode.OK;
                return response;
            }
            catch (ResourceTypeNotSupportedException resourceTypeNotSupportedException)
            {
                return StatusCode((int)HttpStatusCode.NotImplemented, new ResourceDiscoveryFailedDetails { Details=$"Resource type '{resourceTypeNotSupportedException.ResourceType}' for discovery group '{resourceDiscoveryGroup}' is not supported"});
            }
        }
    }
}
