using GuardNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Promitor.Agents.ResourceDiscovery.Configuration;

namespace Promitor.Agents.ResourceDiscovery.Controllers.v1
{
    /// <summary>
    /// API endpoint to interact with resource discovery groups
    /// </summary>
    [ApiController]
    [Route("api/v1/resources/groups")]
    public class ResourceDiscoveryGroupsV1Controller : ControllerBase
    {
        private readonly IOptionsMonitor<ResourceDeclaration> _resourceDeclarationMonitor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourceDiscoveryGroupsV1Controller" /> class.
        /// </summary>
        public ResourceDiscoveryGroupsV1Controller(IOptionsMonitor<ResourceDeclaration> resourceDeclarationMonitor)
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
