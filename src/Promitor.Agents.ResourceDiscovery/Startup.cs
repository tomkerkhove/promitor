using System;
using System.IO;
using System.Linq;
using Arcus.WebApi.Correlation;
using Arcus.WebApi.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Promitor.Agents.Core;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Graph;
using Promitor.Agents.ResourceDiscovery.Health;
using Promitor.Agents.ResourceDiscovery.Repositories;
using Promitor.Agents.Scraper.Extensions;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;
using Swashbuckle.AspNetCore.Filters;

namespace Promitor.Agents.ResourceDiscovery
{
    public class Startup : AgentStartup
    {
        private const string ApiName = "Promitor - Resource Discovery API";
        private const string ComponentName = "Promitor Resource Discovery";

        /// <summary>
        ///     Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRuntimeConfiguration(Configuration);

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.RespectBrowserAcceptHeader = true;

                RestrictToJsonContentType(options);
                AddEnumAsStringRepresentation(options);
            });

            services.AddHealthChecks()
                .AddCheck<AzureResourceGraphHealthCheck>("azure-resource-graph", failureStatus: HealthStatus.Unhealthy);
            services.AddCorrelation();
            services.Configure<ResourceDeclaration>(Configuration);
            services.AddTransient<AzureResourceGraph>();
            services.AddTransient<ResourceRepository>();

            var openApiInformation = new OpenApiInfo
            {
                Title = $"{ApiName} v1",
                Version = "v1"
            };

            var xmlDocumentationPath = GetXmlDocumentationPath(services);
            services.AddSwaggerGen(swaggerGenerationOptions =>
            {
                swaggerGenerationOptions.SwaggerDoc("v1", openApiInformation);

                swaggerGenerationOptions.OperationFilter<AddHeaderOperationFilter>("X-Transaction-Id", "Transaction ID is used to correlate multiple operation calls. A new transaction ID will be generated if not specified.", false);
                swaggerGenerationOptions.OperationFilter<AddResponseHeadersFilter>();

                if (string.IsNullOrEmpty(xmlDocumentationPath) == false)
                    swaggerGenerationOptions.IncludeXmlComments(xmlDocumentationPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCorrelation();
            app.UseRouting();

            app.UseSwagger(swaggerOptions => { swaggerOptions.RouteTemplate = "api/{documentName}/docs.json"; });
            app.UseSwaggerUI(swaggerUiOptions =>
            {
                swaggerUiOptions.SwaggerEndpoint("/api/v1/docs.json", ApiName);
                swaggerUiOptions.RoutePrefix = "api/docs";
                swaggerUiOptions.DocumentTitle = ApiName;
            });
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            
            UseSerilog(ComponentName, app.ApplicationServices);
        }
    }
}