using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Promitor.Scraper.Scheduling;
using Promitor.Scraper.Scheduling.Infrastructure.Extensions;
using Promitor.Scraper.Scheduling.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        public static void UseCronScheduler(this IServiceCollection services)
        {
            services.AddSingleton<IScheduledTask, AzureMonitorScrapingTask>();
            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });
        }

        public static void UseOpenApiSpecifications(this IServiceCollection services, string prometheusScrapeEndpointPath, int apiVersion)
        {
            var openApiInformation = new Info
            {
                Contact = new Contact
                {
                    Name = "Tom Kerkhove",
                    Url = "https://blog.tomkerkhove.be"
                },
                Title = $"Promitor v{apiVersion}",
                Description = $"Collection of APIs to manage the Azure Monitor scrape endpoint for Prometheus.\r\nThe scrape endpoint is exposed at '{prometheusScrapeEndpointPath}'",
                Version = $"v{apiVersion}",
                License = new License
                {
                    Name = "MIT",
                    Url = "https://github.com/tomkerkhove/promitor/LICENSE"
                }
            };

            var xmlDocumentationPath = GetXmlDocumentationPath(services);

            services.AddSwaggerGen(swaggerGenerationOptions =>
            {
                swaggerGenerationOptions.SwaggerDoc($"v{apiVersion}", openApiInformation);
                swaggerGenerationOptions.DescribeAllEnumsAsStrings();

                if (string.IsNullOrEmpty(xmlDocumentationPath) == false)
                {
                    swaggerGenerationOptions.IncludeXmlComments(xmlDocumentationPath);
                }
            });
        }

        private static string GetXmlDocumentationPath(IServiceCollection services)
        {
            var hostingEnvironment = services.FirstOrDefault(service => service.ServiceType == typeof(IHostingEnvironment));
            if (hostingEnvironment == null)
            {
                return string.Empty;
            }

            var contentRootPath = ((IHostingEnvironment) hostingEnvironment.ImplementationInstance).ContentRootPath;
            var xmlDocumentationPath = $"{contentRootPath}\\Docs\\Open-Api.xml";

            return File.Exists(xmlDocumentationPath) ? xmlDocumentationPath : string.Empty;
        }
    }
}