using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Annotations;

namespace Promitor.Agents.Scraper.Controllers.v1
{
    [Route("api/v1/health")]
    public class HealthController : Controller
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        ///     Get Health
        /// </summary>
        /// <remarks>Provides an indication about the health of the scraper</remarks>
        [HttpGet]
        [SwaggerOperation(OperationId = "Health_Get")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Scraper is healthy", Type = typeof(HealthReport))]
        [SwaggerResponse((int)HttpStatusCode.ServiceUnavailable, Description = "Scraper is not healthy")]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();

            return report.Status == HealthStatus.Healthy ? Ok(report) : StatusCode((int)HttpStatusCode.ServiceUnavailable, report);
        }
    }
}