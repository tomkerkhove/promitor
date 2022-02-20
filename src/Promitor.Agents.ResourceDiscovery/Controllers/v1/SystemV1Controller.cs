using System.Net;
using Microsoft.AspNetCore.Mvc;
using Promitor.Agents.Core.Contracts;
using Promitor.Core;
using Swashbuckle.AspNetCore.Annotations;

namespace Promitor.Agents.ResourceDiscovery.Controllers.v1
{
    [Route("api/v1/system")]
    public class SystemV1Controller : Controller
    {
        /// <summary>
        ///     Get System Info
        /// </summary>
        /// <remarks>Provides information concerning the Promitor Resource Discovery Agent</remarks>
        [HttpGet]
        [SwaggerOperation(OperationId = "System_Get")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Information concerning Promitor Resource Discovery Agent is provided", Type = typeof(SystemInfo))]
        [SwaggerResponse((int)HttpStatusCode.ServiceUnavailable, Description = "Unable to process the request due to an server error")]
        public IActionResult Get()
        {
            var version = Version.Get();

            var systemInfo = new SystemInfo
            {
                Version = version
            };

            return Ok(systemInfo);
        }
    }
}