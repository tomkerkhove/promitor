using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using GuardNet;
using Swashbuckle.AspNetCore.Filters;

namespace Promitor.Agents.ResourceDiscovery.Controllers
{
    /// <summary>
    /// API endpoint to check the health of the application.
    /// </summary>
    [ApiController]
    [Route("api/v1/health")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthController"/> class.
        /// </summary>
        /// <param name="healthCheckService">The service to provide the health of the API application.</param>
        public HealthController(HealthCheckService healthCheckService)
        {
            Guard.NotNull(healthCheckService, nameof(healthCheckService));

            _healthCheckService = healthCheckService;
        }

        /// <summary>
        ///     Get Health
        /// </summary>
        /// <remarks>Provides an indication about the health of the API.</remarks>
        /// <response code="200">API is healthy</response>
        /// <response code="503">API is unhealthy or in degraded state</response>
        [HttpGet(Name = "Health_Get")]
        [ProducesResponseType(typeof(HealthReport), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HealthReport), StatusCodes.Status503ServiceUnavailable)]
        [SwaggerResponseHeader(200, "RequestId", "string", "The header that has a request ID that uniquely identifies this operation call")]
        [SwaggerResponseHeader(200, "X-Transaction-Id", "string", "The header that has the transaction ID is used to correlate multiple operation calls.")]
        public async Task<IActionResult> Get()
        {
            HealthReport healthReport = await _healthCheckService.CheckHealthAsync();
            
            if (healthReport?.Status == HealthStatus.Healthy)
            {
                return Ok(healthReport);
            }
            else
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, healthReport);
            }
        }
    }
}
