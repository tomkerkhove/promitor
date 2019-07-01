using System;
using Microsoft.Extensions.Configuration;
using Promitor.Core.Configuration.Prometheus;

namespace Promitor.Core.Scraping
{
    public static class ScrapeEndpoint
    {
        private const string DefaultScrapeEndpoint = "/metrics";

        /// <summary>
        ///     Determines the base path under which the scrape endpoint is configured to be exposed
        /// </summary>
        public static string DetermineBaseUri(IConfiguration configuration)
        {
            var runtimeConfiguration = configuration.GetSection("prometheus:scrapeEndpoint").Get<ScrapeEndpointConfiguration>();

            if (!string.IsNullOrWhiteSpace(runtimeConfiguration?.BaseUriPath))
            {
                return runtimeConfiguration.BaseUriPath;
            }

            Console.WriteLine($"No scraping endpoint was specified, falling back to default '{DefaultScrapeEndpoint}'...");
            return DefaultScrapeEndpoint;
        }
    }
}
