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
                if (string.IsNullOrWhiteSpace(metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath) == false)
                {
                    app.UsePrometheusServer(prometheusOptions =>
                    {
                        prometheusOptions.MapPath = metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath;
                        prometheusOptions.UseDefaultCollectors = false;
                    });
                }
            }

            return app;
        }
    }
}
