﻿using System.Collections.Generic;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Core.Configuration.Authentication;
using Promitor.Core;
using Promitor.Core.Metrics;
using Promitor.Core.Metrics.Sinks;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Integrations.AzureMonitor;
using Promitor.Integrations.AzureMonitor.Configuration;

namespace Promitor.Agents.Scraper
{
    public class AzureMonitorClientFactory
    {
        private readonly Dictionary<string, AzureMonitorClient> _azureMonitorClients = new Dictionary<string, AzureMonitorClient>();

        /// <summary>
        /// Provides an Azure Monitor client
        /// </summary>
        /// <param name="cloud">Name of the Azure cloud to interact with</param>
        /// <param name="tenantId">Id of the tenant that owns the Azure subscription</param>
        /// <param name="subscriptionId">Id of the Azure subscription</param>
        /// <param name="metricSinkWriter">Writer to send metrics to all configured sinks</param>
        /// <param name="metricsCollector">Metrics collector to write metrics to Prometheus</param>
        /// <param name="configuration">Configuration of Promitor</param>
        /// <param name="azureMonitorLoggingConfiguration">Options for Azure Monitor logging</param>
        /// <param name="loggerFactory">Factory to create loggers with</param>
        public AzureMonitorClient CreateIfNotExists(AzureEnvironment cloud, string tenantId, string subscriptionId, MetricSinkWriter metricSinkWriter, IRuntimeMetricsCollector metricsCollector, IConfiguration configuration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, ILoggerFactory loggerFactory)
        {
            if (_azureMonitorClients.ContainsKey(subscriptionId))
            {
                return _azureMonitorClients[subscriptionId];
            }

            var azureMonitorClient = CreateNewAzureMonitorClient(cloud, tenantId, subscriptionId, metricSinkWriter, metricsCollector, configuration, azureMonitorLoggingConfiguration, loggerFactory);
            _azureMonitorClients.TryAdd(subscriptionId, azureMonitorClient);

            return azureMonitorClient;
        }

        private static AzureMonitorClient CreateNewAzureMonitorClient(AzureEnvironment cloud, string tenantId, string subscriptionId, MetricSinkWriter metricSinkWriter, IRuntimeMetricsCollector metricsCollector, IConfiguration configuration, IOptions<AzureMonitorLoggingConfiguration> azureMonitorLoggingConfiguration, ILoggerFactory loggerFactory)
        {
            var azureCredentials = DetermineAzureCredentials(configuration);
            var azureMonitorClient = new AzureMonitorClient(cloud, tenantId, subscriptionId, azureCredentials.AuthenticationMode, azureCredentials.ManagedIdentityId, azureCredentials.ApplicationId, azureCredentials.Secret, azureMonitorLoggingConfiguration, metricSinkWriter, metricsCollector, loggerFactory);
            return azureMonitorClient;
        }

        private static AzureCredentials DetermineAzureCredentials(IConfiguration configuration)
        {
            var authenticationConfiguration = configuration.GetSection("authentication").Get<AuthenticationConfiguration>();

            // To be still compatible with existing infrastructure using previous version of Promitor, we need to check if the authentication section exists.
            // If not, we should use a default value
            if (authenticationConfiguration == null)
            {
                authenticationConfiguration = new AuthenticationConfiguration();
            }

            var applicationId = configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationId);
            var managedIdentityId = configuration.GetValue<string>(EnvironmentVariables.Authentication.ManagedIdentityId);
            var applicationKey = configuration.GetValue<string>(EnvironmentVariables.Authentication.ApplicationKey);

            return new AzureCredentials
            {
                AuthenticationMode = authenticationConfiguration.Mode,
                ManagedIdentityId = managedIdentityId,
                ApplicationId = applicationId,
                Secret = applicationKey
            };
        }
    }
}
