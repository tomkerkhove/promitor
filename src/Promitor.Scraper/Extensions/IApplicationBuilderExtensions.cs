using Prometheus.Client.Owin;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    // ReSharper disable once InconsistentNaming
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Add support for Open API & Develerop UI
        /// </summary>
        public static void UseOpenApiUi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(swaggerUiOptions =>
            {
                swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Promitor API");
                swaggerUiOptions.DisplayOperationId();
                swaggerUiOptions.EnableDeepLinking();
                swaggerUiOptions.DocumentTitle = "Promitor API";
                swaggerUiOptions.DocExpansion(DocExpansion.List);
                swaggerUiOptions.DisplayRequestDuration();
                swaggerUiOptions.EnableFilter();
            });
        }

        /// <summary>
        ///     Add support for exposing a prometheus scaraping endpoint
        /// </summary>
        /// <param name="scrapeEndpointPath">Path where the scrape endpoint will be exposed</param>
        public static IApplicationBuilder UsePrometheusScraper(this IApplicationBuilder app, string scrapeEndpointPath)
        {
            if (scrapeEndpointPath.StartsWith("/"))
            {
                scrapeEndpointPath = scrapeEndpointPath.Substring(1);
            }
            
            var prometheusOptions = new PrometheusOptions
            {
                MapPath = scrapeEndpointPath,
                UseDefaultCollectors = false
            };

            app.UsePrometheusServer(prometheusOptions);
            
            return app;
        }
    }
}