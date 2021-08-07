﻿using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Prometheus.Client.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

// ReSharper disable once CheckNamespace
namespace Promitor.Agents.Scraper.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Use prometheus for writing metrics
        /// </summary>
        public static IServiceCollection AddPrometheusMetrics(this IServiceCollection services)
        {
            services.AddMetricFactory();
            return services;
        }

        /// <summary>
        ///     Expose services as Web API
        /// </summary>
        public static IServiceCollection UseWebApi(this IServiceCollection services)
        {
            services.AddRouting(options =>
                {
                    options.LowercaseUrls = true;
                    options.LowercaseQueryStrings = true;
                })
                .AddControllers(options =>
                {
                    options.ReturnHttpNotAcceptable = true;
                    options.RespectBrowserAcceptHeader = true;

                    RestrictToJsonContentType(options);
                })
                .AddJsonOptions(jsonOptions =>
                {
                    jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    jsonOptions.JsonSerializerOptions.IgnoreNullValues = true;
                })
                .AddNewtonsoftJson(jsonOptions =>
                {
                    jsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter());
                    jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            return services;
        }

        /// <summary>
        ///     Use OpenAPI specification
        /// </summary>
        /// <param name="services">Collections of services in application</param>
        /// <param name="title">Title of the application</param>
        /// <param name="description">Description of the application</param>
        /// <param name="apiVersion">Version of the API</param>
        public static IServiceCollection UseOpenApiSpecifications(this IServiceCollection services, string title, string description, int apiVersion)
        {
            var openApiInformation = new OpenApiInfo
            {
                Contact = new OpenApiContact
                {
                    Name = "Tom Kerkhove",
                    Url = new Uri("https://blog.tomkerkhove.be")
                },
                Title = title,
                Description = description,
                Version = $"v{apiVersion}",
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://github.com/tomkerkhove/promitor/LICENSE")
                }
            };

            var xmlDocumentationPath = GetXmlDocumentationPath(services);

            services.AddSwaggerGen(swaggerGenerationOptions =>
            {
                swaggerGenerationOptions.EnableAnnotations();
                swaggerGenerationOptions.SwaggerDoc($"v{apiVersion}", openApiInformation);

                swaggerGenerationOptions.OperationFilter<AddHeaderOperationFilter>("X-Transaction-Id", "Transaction ID is used to correlate multiple operation calls. A new transaction ID will be generated if not specified.", false);
                swaggerGenerationOptions.OperationFilter<AddResponseHeadersFilter>();

                if (string.IsNullOrEmpty(xmlDocumentationPath) == false)
                {
                    swaggerGenerationOptions.IncludeXmlComments(xmlDocumentationPath);
                }
            });

            return services;
        }

        private static void RestrictToJsonContentType(MvcOptions options)
        {
            var allButJsonInputFormatters = options.InputFormatters.Where(formatter => !(formatter is SystemTextJsonInputFormatter));
            foreach (IInputFormatter inputFormatter in allButJsonInputFormatters)
            {
                options.InputFormatters.Remove(inputFormatter);
            }

            // Removing for text/plain, see https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting?view=aspnetcore-3.0#special-case-formatters
            options.OutputFormatters.RemoveType<StringOutputFormatter>();
        }

        private static string GetXmlDocumentationPath(IServiceCollection services, string docFileName = "Open-Api.xml")
        {
            var hostingEnvironment = services.FirstOrDefault(service => service.ServiceType == typeof(IWebHostEnvironment));
            if (hostingEnvironment == null)
                return string.Empty;

            var contentRootPath = ((IWebHostEnvironment) hostingEnvironment.ImplementationInstance).ContentRootPath;
            var xmlDocumentationPath = $"{contentRootPath}/Docs/{docFileName}";

            return File.Exists(xmlDocumentationPath) ? xmlDocumentationPath : string.Empty;
        }
    }
}