using System;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

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
        /// <param name="apiName">Name of API</param>
        /// <param name="openApiUiConfigurationAction">Action to configure Open API UI</param>
        /// <param name="openApiConfigurationAction">Action to configure Open API</param>
        public static IApplicationBuilder ExposeOpenApiUi(this IApplicationBuilder app, string apiName = null, Action<SwaggerUIOptions> openApiUiConfigurationAction = null, Action<SwaggerOptions> openApiConfigurationAction = null)
        {
            if (openApiConfigurationAction == null)
            {
                openApiConfigurationAction = setupAction => setupAction.RouteTemplate = "api/{documentName}/docs.json";
            }

            if (openApiUiConfigurationAction == null)
            {
                openApiUiConfigurationAction = swaggerUiOptions =>
                {
                    swaggerUiOptions.ConfigureDefaultOptions(apiName);
                    swaggerUiOptions.SwaggerEndpoint("/api/v1/docs.json", apiName);
                    swaggerUiOptions.RoutePrefix = "api/docs";
                };
            }

            // New Swagger UI
            app.UseSwagger(openApiConfigurationAction);
            app.UseSwaggerUI(openApiUiConfigurationAction);

            return app;
        }
    }
}
