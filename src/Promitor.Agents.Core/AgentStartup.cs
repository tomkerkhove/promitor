using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using GuardNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Promitor.Agents.Core
{
    public class AgentStartup
    {
        /// <summary>
        ///     Configuration of the application
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="configuration">Configuration of the application</param>
        public AgentStartup(IConfiguration configuration)
        {
            Guard.NotNull(configuration, nameof(configuration));

            Configuration = configuration;
        }

        /// <summary>
        /// Use & wire up Serilog with a configuration
        /// </summary>
        /// <param name="componentName">Name of the component which is starting up</param>
        /// <param name="serviceProvider">Registered services in the application</param>
        protected void UseSerilog(string componentName, IServiceProvider serviceProvider)
        {
            Log.Logger = CreateSerilogConfiguration(componentName, serviceProvider).CreateLogger();
        }

        /// <summary>
        /// Creates a configuration for Serilog
        /// </summary>
        /// <param name="componentName">Name of the component which is starting up</param>
        /// <param name="serviceProvider">Registered services in the application</param>
        protected virtual LoggerConfiguration CreateSerilogConfiguration(string componentName, IServiceProvider serviceProvider)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithComponentName(componentName)
                .Enrich.WithVersion()
                .Enrich.WithHttpCorrelationInfo(serviceProvider)
                .WriteTo.Console()
                .WriteTo.AzureApplicationInsights(instrumentationKey);
        }

        protected string GetXmlDocumentationPath(IServiceCollection services, string docFileName = "Open-Api.xml")
        {
            var hostingEnvironment = services.FirstOrDefault(service => service.ServiceType == typeof(IWebHostEnvironment));
            if (hostingEnvironment == null)
                return string.Empty;

            var contentRootPath = ((IWebHostEnvironment)hostingEnvironment.ImplementationInstance).ContentRootPath;
            var xmlDocumentationPath = $"{contentRootPath}/Docs/{docFileName}";

            return File.Exists(xmlDocumentationPath) ? xmlDocumentationPath : string.Empty;
        }

        protected void RestrictToJsonContentType(MvcOptions options)
        {
            var allButJsonInputFormatters = options.InputFormatters.Where(formatter => !(formatter is SystemTextJsonInputFormatter));
            foreach (IInputFormatter inputFormatter in allButJsonInputFormatters)
            {
                options.InputFormatters.Remove(inputFormatter);
            }

            // Removing for text/plain, see https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting?view=aspnetcore-3.0#special-case-formatters
            options.OutputFormatters.RemoveType<StringOutputFormatter>();
        }

        protected void AddEnumAsStringRepresentation(MvcOptions options)
        {
            var onlyJsonInputFormatters = options.InputFormatters.OfType<SystemTextJsonInputFormatter>();
            foreach (SystemTextJsonInputFormatter inputFormatter in onlyJsonInputFormatters)
            {
                inputFormatter.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }

            var onlyJsonOutputFormatters = options.OutputFormatters.OfType<SystemTextJsonOutputFormatter>();
            foreach (SystemTextJsonOutputFormatter outputFormatter in onlyJsonOutputFormatters)
            {
                outputFormatter.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }
        }
    }
}