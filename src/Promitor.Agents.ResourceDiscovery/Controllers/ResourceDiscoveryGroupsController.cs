using Microsoft.AspNetCore.Mvc;
using GuardNet;
using Microsoft.Extensions.Options;
using Promitor.Agents.ResourceDiscovery.Configuration;

namespace Promitor.Agents.ResourceDiscovery.Controllers
{
    /// <summary>
    /// API endpoint to interact with resource discovery groups
    /// </summary>
    [ApiController]
    [Route("api/v1/resources/groups")]
    public class ResourceDiscoveryGroupsController : ControllerBase
    {
        private readonly IOptionsMonitor<ResourceDeclaration> _resourceDeclarationMonitor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourceDiscoveryGroupsController" /> class.
        /// </summary>
        public ResourceDiscoveryGroupsController(IOptionsMonitor<ResourceDeclaration> resourceDeclarationMonitor)
        {
            Guard.NotNull(resourceDeclarationMonitor, nameof(resourceDeclarationMonitor));

            _resourceDeclarationMonitor = resourceDeclarationMonitor;
        }

        /// <summary>
        ///     Get Resource Discovery Groups
        /// </summary>
        /// <remarks>Lists all available resource discovery groups.</remarks>
        [HttpGet(Name = "ResourceDiscoveryGroups_Get")]
        public IActionResult Get()
        {
            var resourceDeclaration = _resourceDeclarationMonitor.CurrentValue;
            return Ok(resourceDeclaration.ResourceDiscoveryGroups);
        }
    }
}
