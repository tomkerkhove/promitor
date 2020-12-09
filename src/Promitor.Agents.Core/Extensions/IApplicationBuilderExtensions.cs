using System;
using Microsoft.Extensions.Configuration;
using Promitor.Agents.Core.Middleware;
using Promitor.Core;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    // ReSharper disable once InconsistentNaming
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds middleware to automatically add the version in our responses
        /// </summary>
        public static IApplicationBuilder UseVersionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<AgentVersionMiddleware>();

            return app;
        }

        private const string OpenApiEndpointPath = "api/v1/docs.json";

        /// <summary>
        ///     Add support for Open API with API explorer
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <param name="apiName">Name of API</param>
        /// <param name="openApiUiConfigurationAction">Action to configure Open API UI</param>
        /// <param name="openApiConfigurationAction">Action to configure Open API</param>
        public static IApplicationBuilder ExposeOpenApiUi(this IApplicationBuilder app, IConfiguration configuration, string apiName = null, Action<SwaggerUIOptions> openApiUiConfigurationAction = null, Action<SwaggerOptions> openApiConfigurationAction = null)
        {
            if (openApiConfigurationAction == null)
            {
                openApiConfigurationAction = setupAction => setupAction.RouteTemplate = "api/{documentName}/docs.json";
            }

            if (openApiUiConfigurationAction == null)
            {
                var apiBasePath = configuration.GetValue(EnvironmentVariables.Api.Prefix, defaultValue: string.Empty);
                if (apiBasePath.StartsWith("/") == false)
                {
                    apiBasePath = $"/{apiBasePath}";
                }

                var openApiEndpoint = apiBasePath.EndsWith("/") ? $"{apiBasePath}{OpenApiEndpointPath}" : $"{apiBasePath}/{OpenApiEndpointPath}";
                openApiUiConfigurationAction = swaggerUiOptions =>
                {
                    swaggerUiOptions.ConfigureDefaultOptions(apiName);
                    swaggerUiOptions.SwaggerEndpoint(openApiEndpoint, apiName);
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
