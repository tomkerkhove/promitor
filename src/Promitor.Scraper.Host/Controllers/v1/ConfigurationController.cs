using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Promitor.Scraper.Host.Controllers.v1
{
    [Route("api/v1/configuration")]
    public class ConfigurationController : Controller
    {
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;

        public ConfigurationController(IMetricsDeclarationProvider metricsDeclarationProvider)
        {
            _metricsDeclarationProvider = metricsDeclarationProvider;
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
            var scrapeConfiguration = _metricsDeclarationProvider.Get();
            return scrapeConfiguration.Metrics;
        }
    }
}