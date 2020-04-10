using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Promitor.Core.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Runtime.Agents.Scraper.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace Promitor.Runtime.Agents.Scraper.Controllers.v1
{
    [Route("api/v1/configuration")]
    public class ConfigurationController : Controller
    {
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IOptionsMonitor<RuntimeConfiguration> _runtimeConfiguration;

        public ConfigurationController(IOptionsMonitor<RuntimeConfiguration> runtimeConfiguration, IMetricsDeclarationProvider metricsDeclarationProvider)
        {
            _runtimeConfiguration = runtimeConfiguration;
            _metricsDeclarationProvider = metricsDeclarationProvider;
        }

        /// <summary>
        ///     Get Metrics Declaration
        /// </summary>
        /// <remarks>Provides a list of all configured metrics to scrape</remarks>
        [HttpGet]
        [Route("metric-declaration")]
        [SwaggerOperation(OperationId = "Configuration_GetMetricDeclaration")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Configuration concerning the metrics to scrape",
            Type = typeof(List<MetricDefinition>))]
        [SwaggerResponse((int)HttpStatusCode.NoContent, Description = "No configured metrics were found to scrape")]
        public IActionResult GetMetricDeclaration()
        {
            var scrapeConfiguration = _metricsDeclarationProvider.Get(true);
            return Ok(scrapeConfiguration.Metrics);
        }

        /// <summary>
        ///     Get Runtime Configuration
        /// </summary>
        /// <remarks>Provides the runtime configuration</remarks>
        [HttpGet]
        [Route("runtime")]
        [SwaggerOperation(OperationId = "Configuration_GetRuntime")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Configuration concerning the runtime",
            Type = typeof(RuntimeConfiguration))]
        public IActionResult GetRuntime()
        {
            var runtimeConfig = _runtimeConfiguration.CurrentValue.Clone();

            if (string.IsNullOrWhiteSpace(runtimeConfig?.Telemetry?.ApplicationInsights?.InstrumentationKey) == false)
            {
                runtimeConfig.Telemetry.ApplicationInsights.InstrumentationKey = "***";
            }

            return Ok(runtimeConfig);
        }
    }
}