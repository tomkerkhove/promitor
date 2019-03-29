using System.Net;
using Microsoft.AspNetCore.Mvc;
using Promitor.Scraper.Host.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Promitor.Scraper.Host.Controllers.v1
{
    [Route("api/v1/health")]
    public class HealthController : Controller
    {
        /// <summary>
        ///     Get Health
        /// </summary>
        /// <remarks>Provides an indication about the health of the scraper</remarks>
        [HttpGet]
        [SwaggerOperation(OperationId = "Health_Get")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Scraper is healthy, with subscription read limit")]
        [SwaggerResponse((int)HttpStatusCode.ServiceUnavailable, Description = "Scraper is not healthy")]
        public IActionResult Get()
        {
            int subscriptionReadLimit = HealthMonitor.Instance.subscriptionLimitCount;
            return Ok(subscriptionReadLimit);
        }
    }
}