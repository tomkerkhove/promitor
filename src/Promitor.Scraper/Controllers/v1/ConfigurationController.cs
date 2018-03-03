using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Promitor.Scraper.Controllers.v1
{
    [Route("api/v1/[controller]")]
    public class ConfigurationController : Controller
    {
        /// <summary>
        ///     Provides a list of all configured metrics to scrape
        /// </summary>
        [HttpGet]
        [SwaggerOperation("Get Metrics Configuration")]
        [SwaggerResponse((int) HttpStatusCode.OK, Description = "Metrics to scrape")]
        public IEnumerable<string> Get()
        {
            return new[] {"value1", "value2"};
        }
    }
}