using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Configuration.Server;
using Promitor.Agents.Core.Configuration.Telemetry;
using Promitor.Agents.Core.Configuration.Telemetry.Sinks;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Graph;
using Promitor.Agents.ResourceDiscovery.Graph.Interfaces;
using Promitor.Agents.ResourceDiscovery.Repositories;
using Promitor.Agents.ResourceDiscovery.Repositories.Interfaces;
using Promitor.Agents.ResourceDiscovery.Scheduling;
using Promitor.Agents.ResourceDiscovery.Usability;
using Promitor.Agents.ResourceDiscovery.Validation.Steps;
using Promitor.Core.Metrics;
using Promitor.Integrations.Azure.Authentication.Configuration;
using Promitor.Integrations.Sinks.Prometheus;

namespace Promitor.Agents.ResourceDiscovery.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        ///     Inject configuration
        /// </summary>
        public static IServiceCollection AddRuntimeConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AgentRuntimeConfiguration>(configuration);
            services.Configure<CacheConfiguration>(configuration.GetSection("cache"));
            services.Configure<TelemetryConfiguration>(configuration.GetSection("telemetry"));
            services.Configure<ServerConfiguration>(configuration.GetSection("server"));
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("authentication"));
            services.Configure<ApplicationInsightsConfiguration>(configuration.GetSection("telemetry:applicationInsights"));
            services.Configure<ContainerLogConfiguration>(configuration.GetSection("telemetry:containerLogs"));

            return services;
        }
        /// <summary>
        ///     Inject configuration
        /// </summary>
        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScheduler(builder =>
            {
                var jobName = "Azure Landscape Discovery";
                builder.AddJob<AzureLandscapeDiscoveryBackgroundJob>(configure: schedulerOptions =>
                    {
                        schedulerOptions.CronSchedule = "*/5 * * * *";
                        schedulerOptions.RunImmediately = true;
                    });
                builder.UnobservedTaskExceptionHandler = (sender, exceptionEventArgs) => UnobservedJobHandler(jobName, exceptionEventArgs, services);
            });

            return services;
        }

        private static void UnobservedJobHandler(object jobName, UnobservedTaskExceptionEventArgs exceptionEventArgs, IServiceCollection services)
        {
            var logger = services.FirstOrDefault(service => service.ServiceType == typeof(ILogger));
            var loggerInstance = (ILogger)logger?.ImplementationInstance;

            loggerInstance?.LogCritical(exceptionEventArgs.Exception, "Unhandled exception in job {JobName}", jobName);

            exceptionEventArgs.SetObserved();
        }

        /// <summary>
        ///     Add Azure Resource Graph integration
        /// </summary>
        public static IServiceCollection AddAzureResourceGraph(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ResourceDeclaration>(configuration);
            services.Configure<AzureLandscape>(configuration.GetSection("azureLandscape"));
            services.Configure<List<ResourceDiscoveryGroup>>(configuration.GetSection("resourceDiscoveryGroups"));
            services.AddTransient<IAzureResourceGraph, AzureResourceGraph>();
            services.AddTransient<ICachedAzureResourceGraph, CachedAzureResourceGraph>();

            var isCacheEnabled = configuration.GetValue<bool>("cache:enabled", defaultValue: true);
            if (isCacheEnabled)
            {
                services.AddTransient<AzureResourceRepository>();
                services.AddTransient<IAzureResourceRepository, CachedAzureResourceRepository>();
            }
            else
            {
                services.AddTransient<IAzureResourceRepository, AzureResourceRepository>();
            }

            return services;
        }

        /// <summary>
        ///     Add validation rules
        /// </summary>
        public static IServiceCollection AddValidationRules(this IServiceCollection services)
        {
            services.AddTransient<IValidationStep, AzureLandscapeValidationStep>();
            services.AddTransient<IValidationStep, ResourceDiscoveryGroupValidationStep>();
            services.AddTransient<IValidationStep, AzureAuthenticationValidationStep>();
            services.AddTransient<RuntimeValidator>();

            return services;
        }

        /// <summary>
        ///     Add usability
        /// </summary>
        public static IServiceCollection AddUsability(this IServiceCollection services)
        {
            services.AddTransient<DiscoveryGroupTableGenerator>();

            return services;
        }
    }
}