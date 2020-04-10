using Microsoft.AspNetCore.Builder;
using Prometheus.Client.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerUI;

// ReSharper disable once CheckNamespace
namespace Promitor.Runtime.Agents.Scraper.Extensions
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

        /// <summary>
        ///     Add support for Open API with API explorer
        /// </summary>
        /// <param name="app">Application Builder</param>
        public static IApplicationBuilder ExposeOpenApiUi(this IApplicationBuilder app)
        {
            // New Swagger UI
            app.UseSwagger(setupAction => setupAction.RouteTemplate = "api/{documentName}/docs.json");
            app.UseSwaggerUI(swaggerUiOptions =>
               {
                   swaggerUiOptions.ConfigureDefaultOptions();
                   swaggerUiOptions.SwaggerEndpoint("/api/v1/docs.json", "Promitor API");
                   swaggerUiOptions.RoutePrefix = "api/docs";
               });

            // Deprecated Swagger UI
            app.UseSwagger(setupAction => setupAction.SerializeAsV2 = true)
               .UseSwaggerUI(swaggerUiOptions =>
               {
                   swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Promitor API");
                   swaggerUiOptions.SwaggerEndpoint("/api/v1/docs.json", "Promitor API (OpenAPI 3.0)");
                   swaggerUiOptions.ConfigureDefaultOptions();
               });

            return app;
        }
    }
}
