using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Swashbuckle.AspNetCore.Swagger;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        public static void UseOpenApiSpecifications(this IServiceCollection services, int apiVersion)
        {
            var openApiInformation = new Info
            {
                Contact = new Contact
                {
                    Name = "Tom Kerkhove",
                    Url = "https://blog.tomkerkhove.be"
                },
                Title = $"Promitor v{apiVersion}",
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