using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        /// <param name="logger">Logger to write logs to</param>
        public static IApplicationBuilder UseMetricSinks(this IApplicationBuilder app, IConfiguration configuration, ILogger<Startup> logger)
        {
            var metricSinkConfiguration = configuration.GetSection("metricSinks").Get<MetricSinkConfiguration>();
            if (metricSinkConfiguration?.PrometheusScrapingEndpoint != null)
            {
                if (string.IsNullOrWhiteSpace(metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath) == false)
                {
                    logger.LogInformation("Adding Prometheus sink to expose on {PrometheusUrl}", metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath);
                    app.UsePrometheusMetrics(metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath, logger);
                }
            }

            return app;
        }
    }
}
