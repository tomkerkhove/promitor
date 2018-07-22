using Prometheus.Client.AspNetCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    // ReSharper disable once InconsistentNaming
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Add support for Open API with API explorer
        /// </summary>
        /// <param name="app">Application Builder</param>
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