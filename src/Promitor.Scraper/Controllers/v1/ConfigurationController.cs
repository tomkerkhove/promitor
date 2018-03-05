using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Promitor.Scraper.Configuration.Model;
using Promitor.Scraper.Configuration.Providers.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Promitor.Scraper.Controllers.v1
{
    [Route("api/v1/configuration")]
    public class ConfigurationController : Controller
    {
        private readonly IScrapeConfigurationProvider scrapeConfigurationProvider;

        public ConfigurationController(IScrapeConfigurationProvider scrapeConfigurationProvider)
        {
            this.scrapeConfigurationProvider = scrapeConfigurationProvider;
        }

        /// <summary>
        ///     Provides a list of all configured metrics to scrape
        /// </summary>
        [HttpGet]
        [SwaggerOperation("Get Scrape Configuration")]
        [SwaggerResponse((int) HttpStatusCode.OK, Description = "Configuration concerning the metrics to scrape", Type = typeof(List<Metric>))]
        public IEnumerable<Metric> Get()
        {
            var scrapeConfiguration = scrapeConfigurationProvider.GetConfiguration();
            return scrapeConfiguration.Metrics;
        }
    }
}