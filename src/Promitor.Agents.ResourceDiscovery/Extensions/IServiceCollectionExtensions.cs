using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
using Promitor.Agents.ResourceDiscovery.Usability;
using Promitor.Agents.ResourceDiscovery.Validation.Steps;
using Promitor.Integrations.Azure.Authentication.Configuration;

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
                services.AddTransient<ResourceRepository>();
                services.AddTransient<IResourceRepository, CachedResourceRepository>();
            }
            else
            {
                services.AddTransient<IResourceRepository, ResourceRepository>();
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