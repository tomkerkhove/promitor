using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Promitor.Agents.Core.Controllers;
using Swashbuckle.AspNetCore.Filters;

namespace Promitor.Agents.ResourceDiscovery.Controllers.v1
{
    /// <summary>
    /// API endpoint to check the health of the application.
    /// </summary>
    [ApiController]
    [Route("api/v1/health")]
    public class HealthV1Controller : OperationsController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthV1Controller"/> class.
        /// </summary>
        /// <param name="healthCheckService">The service to provide the health of the API application.</param>
        public HealthV1Controller(HealthCheckService healthCheckService)
            : base(healthCheckService)
        {
        }

        /// <summary>
        ///     Get Health
        /// </summary>
        /// <remarks>Provides an indication about the health of the API.</remarks>
        /// <param name="includeDependencies">
        ///     Indication whether or not dependencies integration should be verified.
        ///     Do note that this will contact all dependencies which can have performance impact or cascading failures when
        ///     consumed very often.
        /// </param>
        /// <response code="200">API is healthy</response>
        /// <response code="503">API is unhealthy or in degraded state</response>
        [HttpGet(Name = "Health_Get")]
        [ProducesResponseType(typeof(HealthReport), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HealthReport), StatusCodes.Status503ServiceUnavailable)]
        [SwaggerResponseHeader(new[] { 200, 503 }, "RequestId", "string", "The header that has a request ID that uniquely identifies this operation call")]
        [SwaggerResponseHeader(new[] { 200, 503 }, "X-Transaction-Id", "string", "The header that has the transaction ID is used to correlate multiple operation calls.")]
        public async Task<IActionResult> Get(bool includeDependencies = true)
        {
            return await GetHealthAsync(includeDependencies);
        }
    }
}
