using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Promitor.Scraper.Configuration.Providers.Interfaces;
using Promitor.Scraper.Model.Configuration.Metrics;
using Swashbuckle.AspNetCore.Annotations;

namespace Promitor.Scraper.Controllers.v1
{
    [Route("api/v1/configuration")]
    public class ConfigurationController : Controller
    {
        private readonly IMetricsDeclarationProvider metricsDeclarationProvider;

        public ConfigurationController(IMetricsDeclarationProvider metricsDeclarationProvider)
        {
            this.metricsDeclarationProvider = metricsDeclarationProvider;
        }

        /// <summary>
        ///     Get Metrics Declaration
        /// </summary>
        /// <remarks>Provides a list of all configured metrics to scrape</remarks>
        [HttpGet]
        [Route("metric-declaration")]
        [SwaggerOperation(OperationId = "Configuration_Get")]
        [SwaggerResponse((int) HttpStatusCode.OK, Description = "Configuration concerning the metrics to scrape",
            Type = typeof(List<MetricDefinition>))]
        [SwaggerResponse((int) HttpStatusCode.NoContent, Description = "No configured metrics were found to scrape")]
        public IEnumerable<MetricDefinition> Get()
        {
            var scrapeConfiguration = metricsDeclarationProvider.Get();
            return scrapeConfiguration.Metrics;
        }
    }
}