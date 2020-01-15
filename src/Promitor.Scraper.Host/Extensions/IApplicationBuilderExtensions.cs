using System;
using Microsoft.AspNetCore.Builder;
using Prometheus.Client.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

// ReSharper disable once CheckNamespace
namespace Promitor.Scraper.Host.Extensions
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
            app.UseOpenApi(setupAction => setupAction.RouteTemplate = "api/{documentName}/docs.json")
               .UseOpenApiUi(swaggerUiOptions =>
               {
                   swaggerUiOptions.ConfigureDefaultOptions();
                   swaggerUiOptions.SwaggerEndpoint("/api/v1/docs.json", "Promitor API");
                   swaggerUiOptions.RoutePrefix = "api/docs";
               });

            // Deprecated Swagger UI
            app.UseOpenApi(setupAction => setupAction.SerializeAsV2 = true)
               .UseOpenApiUi(swaggerUiOptions =>
               {
                   swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Promitor API");
                   swaggerUiOptions.SwaggerEndpoint("/api/v1/docs.json", "Promitor API (OpenAPI 3.0)");
                   swaggerUiOptions.ConfigureDefaultOptions();
               });

            return app;
        }

        // This is based on Swashbuckles' UseOpenApi, but they are reusing existing SwaggerOptions which is not ideal since we want to support OpenAPI 2.0 & 3.0
        private static IApplicationBuilder UseOpenApi(this IApplicationBuilder app, Action<SwaggerOptions> setupAction = null)
        {
            SwaggerOptions swaggerOptions = new SwaggerOptions();
            setupAction?.Invoke(swaggerOptions);

            app.UseMiddleware<SwaggerMiddleware>((object)swaggerOptions);

            return app;
        }

        // This is based on Swashbuckles' UseOpenApi, but they are reusing existing SwaggerUIOptions which is not ideal since we want to support OpenAPI 2.0 & 3.0
        private static void UseOpenApiUi(this IApplicationBuilder app, Action<SwaggerUIOptions> setupAction)
        {
            SwaggerUIOptions swaggerUiOptions = new SwaggerUIOptions();
            setupAction?.Invoke(swaggerUiOptions);
            app.UseMiddleware<SwaggerUIMiddleware>((object)swaggerUiOptions);
        }
    }
}
