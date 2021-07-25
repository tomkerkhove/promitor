using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Tests.Integration.Clients;
using Promitor.Tests.Integration.Infrastructure;

namespace Promitor.Tests.Integration.Data
{
    public class AvailableMetricsTestInputGenerator : IEnumerable<object[]>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        IEnumerator<object[]> IEnumerable<object[]>.GetEnumerator()
        {
            var scraperClient = new ScraperClient(_configuration, _logger);
            var declaration = scraperClient.GetMetricDeclarationAsync().Result;
            return declaration.Select(x => new object[] { x.PrometheusMetricDefinition.Name }).GetEnumerator();
        }

        public IEnumerator GetEnumerator() => GetEnumerator();

        public AvailableMetricsTestInputGenerator()
        {
            _logger = NullLogger<AvailableMetricsTestInputGenerator>.Instance;
            _configuration = ConfigurationFactory.Create();
        }
    }
}
