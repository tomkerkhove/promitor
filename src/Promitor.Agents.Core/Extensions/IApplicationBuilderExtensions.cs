using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Prometheus.Client.AspNetCore;
using Prometheus.Client.HttpRequestDurations;
using Promitor.Agents.Core.Middleware;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    // ReSharper disable once InconsistentNaming
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds the required metric sinks
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="baseUriPath">Uri to expose all metrics on</param>
        /// <param name="logger">Logger to write telemetry to</param>
        public static IApplicationBuilder UsePrometheusMetrics(this IApplicationBuilder app, string baseUriPath, ILogger logger)
        {
            logger.LogInformation("Adding Prometheus sink to expose on {PrometheusUrl}", baseUriPath);
            app.UsePrometheusRequestDurations(requestDurationsOptions =>
            {
                requestDurationsOptions.IncludePath = true;
                requestDurationsOptions.IncludeMethod = true;
                requestDurationsOptions.MetricName = "promitor_runtime_http_request_duration_seconds";
            });
            app.UsePrometheusServer(prometheusOptions =>
            {
                prometheusOptions.MapPath = baseUriPath;
                prometheusOptions.UseDefaultCollectors = true;
                prometheusOptions.MetricPrefixName = "promitor_runtime_";
#pragma warning disable CS0618
                prometheusOptions.AddLegacyMetrics = true;
#pragma warning restore CS0618
            });

            return app;
        }

        /// <summary>
        ///     Adds middleware to automatically add the version in our responses
        /// </summary>
        public static IApplicationBuilder UseVersionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<AgentVersionMiddleware>();

            return app;
        }

        /// <summary>
        ///     Add support for Open API with API explorer
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="apiName">Name of API</param>
        /// <param name="openApiUiConfigurationAction">Action to configure Open API UI</param>
        /// <param name="openApiConfigurationAction">Action to configure Open API</param>
        public static IApplicationBuilder ExposeOpenApiUi(this IApplicationBuilder app, string apiName, Action<SwaggerUIOptions> openApiUiConfigurationAction = null, Action<SwaggerOptions> openApiConfigurationAction = null)
        {
            openApiConfigurationAction ??= setupAction =>
            {
                setupAction.RouteTemplate = "api/{documentName}/docs.json";
                setupAction.PreSerializeFilters.Add((swagger, request) => AutomaticallyBuildApiServerUrl(request, swagger));
            };

            openApiUiConfigurationAction ??= swaggerUiOptions =>
            {
                swaggerUiOptions.ConfigureDefaultOptions(apiName);
                swaggerUiOptions.SwaggerEndpoint("../v1/docs.json", apiName);
                swaggerUiOptions.RoutePrefix = "api/docs";
            };

            // New Swagger UI
            app.UseSwagger(openApiConfigurationAction);
            app.UseSwaggerUI(openApiUiConfigurationAction);

            return app;
        }

        private static void AutomaticallyBuildApiServerUrl(HttpRequest request, OpenApiDocument swagger)
        {
            // Default to simple scenario
            var serverUrl = $"{request.Scheme}://{request.Host}";

            // If request forwarding is used, we need to see if we need to adapt
            // Here we need to use the host and prefix, when specified
            // This is required when using reverse proxies such as Azure API Management, Traefik, NGINX, etc.
            if (request.Headers.TryGetValue("X-Forwarded-Host", out var forwardedHost))
            {
                var urlPrefix = string.Empty;
                var prefixFromHeaders = request.Headers["X-Forwarded-Prefix"];

                if (string.IsNullOrWhiteSpace(prefixFromHeaders) == false)
                {
                    urlPrefix = prefixFromHeaders;
                    if (urlPrefix.StartsWith("/") == false)
                    {
                        urlPrefix = $"/{urlPrefix}";
                    }
                }

                serverUrl = $"{request.Scheme}://{forwardedHost}{urlPrefix}";
            }

            swagger.Servers = new List<OpenApiServer> {new() {Url = serverUrl}};
        }
    }
}
