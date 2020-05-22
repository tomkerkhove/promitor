using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Prometheus.Client.AspNetCore;
using Promitor.Agents.Scraper.Configuration.Sinks;

// ReSharper disable once CheckNamespace
namespace Promitor.Agents.Scraper.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds the required metric sinks
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="configuration">Configuration of the scraper agent</param>
        public static IApplicationBuilder UseMetricSinks(this IApplicationBuilder app, IConfiguration configuration)
        {
            var metricSinkConfiguration = configuration.GetSection("metricSinks").Get<MetricSinkConfiguration>();
            if (metricSinkConfiguration?.PrometheusScrapingEndpoint != null)
            {
                AddPrometheusScraperMetricSink(app, metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath);
            }

            return app;
        }

        /// <summary>
        ///     Add support for exposing a prometheus scraping endpoint
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="scrapeEndpointPath">Path where the scrape endpoint will be exposed</param>
        public static IApplicationBuilder AddPrometheusScraperMetricSink(this IApplicationBuilder app, string scrapeEndpointPath)
        {
            if (string.IsNullOrWhiteSpace(scrapeEndpointPath) == false)
            {
                app.UsePrometheusServer(prometheusOptions =>
                {
                    prometheusOptions.MapPath = scrapeEndpointPath;
                    prometheusOptions.UseDefaultCollectors = false;
                });
            }

            return app;
        }
    }
}
