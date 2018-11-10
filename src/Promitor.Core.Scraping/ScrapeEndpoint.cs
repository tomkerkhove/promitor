using System;
using Microsoft.Extensions.Configuration;

namespace Promitor.Core.Scraping
{
    public static class ScrapeEndpoint
    {
        private const string DefaultScrapeEndpoint = "/prometheus/scrape";

        /// <summary>
        ///     Determines the base path under which the scrape endpoint is configured to be exposed
        /// </summary>
        public static string GetBasePath(IConfiguration configuration)
        {
            var scrapeEndpointPath = configuration.GetValue<string>(EnvironmentVariables.Scraping.Path);
            if (string.IsNullOrWhiteSpace(scrapeEndpointPath))
            {
                Console.WriteLine($"No scraping endpoint was specified, falling back to default '{DefaultScrapeEndpoint}'...");
                return DefaultScrapeEndpoint;
            }

            return scrapeEndpointPath.StartsWith("/") ? scrapeEndpointPath.Substring(1) : scrapeEndpointPath;
        }
    }
}