using Microsoft.AspNetCore.Builder;
using Prometheus.Client.AspNetCore;

// ReSharper disable once CheckNamespace
namespace Promitor.Agents.Scraper.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Add support for exposing a prometheus scraping endpoint
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="scrapeEndpointPath">Path where the scrape endpoint will be exposed</param>
        public static IApplicationBuilder UsePrometheusScraper(this IApplicationBuilder app, string scrapeEndpointPath)
        {
            app.UsePrometheusServer(prometheusOptions =>
            {
                prometheusOptions.MapPath = scrapeEndpointPath;
                prometheusOptions.UseDefaultCollectors = false;
            });

            return app;
        }
    }
}
