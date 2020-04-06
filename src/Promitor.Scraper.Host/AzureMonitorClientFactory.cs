using System.Collections.Generic;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Core;
using Promitor.Core.Configuration.Model.AzureMonitor;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Telemetry.Metrics.Interfaces;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Scraper.Host
{
    public class AzureMonitorClientFactory
    {
        private readonly Dictionary<string, AzureMonitorClient> _azureMonitorClients = new Dictionary<string, AzureMonitorClient>();

        public AzureMonitorClient CreateIfNotExists(AzureEnvironment cloud, string tenantId, string subscriptionId, IRuntimeMetricsCollector runtimeMetricCollector, IConfiguration configuration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, ILoggerFactory loggerFactory, IServiceCollection services)
        {
            if (_azureMonitorClients.ContainsKey(subscriptionId))
            {
                return _azureMonitorClients[subscriptionId];
            }

            var azureMonitorClient = CreateNewAzureMonitorClient(cloud, tenantId, subscriptionId, runtimeMetricCollector, configuration, azureMonitorLoggingConfiguration, loggerFactory);
            //services.AddSingleton(azureMonitorClient);
            _azureMonitorClients.Add(subscriptionId, azureMonitorClient);

            return azureMonitorClient;
        }

        private static AzureMonitorClient CreateNewAzureMonitorClient(AzureEnvironment cloud, string tenantId, string subscriptionId, IRuntimeMetricsCollector runtimeMetricCollector, IConfiguration configuration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, ILoggerFactory loggerFactory)
        {
            var azureCredentials = DetermineAzureCredentials(configuration);
            var azureMonitorClient = new AzureMonitorClient(cloud, tenantId, subscriptionId, azureCredentials.ApplicationId, azureCredentials.Secret, azureMonitorLoggingConfiguration, runtimeMetricCollector, loggerFactory);
            return azureMonitorClient;
        }

        private static AzureCredentials DetermineAzureCredentials(IConfiguration configuration)
        {
            var applicationId = configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationId);
            var applicationKey = configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationKey);

            return new AzureCredentials
            {
                ApplicationId = applicationId,
                Secret = applicationKey
            };
        }
    }
}
