using Prometheus.Client.Owin;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class IApplicationBuilderExtensions
    {
        public static void UsePrometheusScraper(this IApplicationBuilder app)
        {
            var prometheusOptions = new PrometheusOptions
            {
                MapPath = "prometheus/scrape"
            };

            app.UsePrometheusServer(prometheusOptions);
        }
    }
}