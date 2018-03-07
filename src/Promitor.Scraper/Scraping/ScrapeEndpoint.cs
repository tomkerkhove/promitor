using Microsoft.Extensions.Configuration;
using Promitor.Scraper.Configuration;

namespace Promitor.Scraper.Scraping
{
    // TODO: Write unit tests for all scenarios
    public static class ScrapeEndpoint
    {
        /// <summary>
        ///     Determines the base path under which the scrape endpoint is configured to be exposed
        /// </summary>
        public static string GetBasePath(IConfiguration configuration)
        {
            var scrapeEndpointPath = configuration.GetValue<string>(EnvironmentVariables.ScrapeEndpointPath);
            if (string.IsNullOrWhiteSpace(scrapeEndpointPath))
            {
                return "/prometheus/scrape";
            }

            return scrapeEndpointPath.StartsWith("/") == false ? $"/{scrapeEndpointPath}" : scrapeEndpointPath;
        }
    }
}