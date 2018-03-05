using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Promitor.Scraper.Configuration.Model;
using Swashbuckle.AspNetCore.SwaggerGen;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Promitor.Scraper.Controllers.v1
{
    [Route("api/v1/configuration")]
    public class ConfigurationController : Controller
    {
        /// <summary>
        ///     Provides a list of all configured metrics to scrape
        /// </summary>
        [HttpGet]
        [SwaggerOperation("Get Scrape Configuration")]
        [SwaggerResponse((int) HttpStatusCode.OK, Description = "Configuration concerning the metrics to scrape")]
        public IEnumerable<Metric> Get()
        {
            var scrapeConfiguration = GetScrapeConfiguration();
            return scrapeConfiguration.Metrics;
        }

        private ScrapeConfiguration GetScrapeConfiguration()
        {
            var rawConfiguragion = System.IO.File.ReadAllText(".\\..\\sample-configuration.yaml");
            var input = new StringReader(rawConfiguragion);

            var builder = new DeserializerBuilder();
            builder.WithNamingConvention(new CamelCaseNamingConvention());
            var deserializer = builder.Build();

            var config = deserializer.Deserialize<ScrapeConfiguration>(input);
            return config;
        }
    }
}