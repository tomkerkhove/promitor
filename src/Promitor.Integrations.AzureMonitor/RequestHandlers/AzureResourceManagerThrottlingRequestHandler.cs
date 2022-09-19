using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.RequestHandlers;
using Promitor.Core;
using Promitor.Core.Metrics.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.Azure.Authentication;
using Version = Promitor.Core.Version;

namespace Promitor.Integrations.AzureMonitor.RequestHandlers
{
    /// <summary>
    ///     Request handler to provide insights on the current api consumption and throttling of Azure Resource Manager
    /// </summary>
    public class AzureResourceManagerThrottlingRequestHandler : ThrottlingRequestHandler
    {
        private readonly Dictionary<string, string> _metricLabels;
        private readonly MetricSinkWriter _metricSinkWriter;
        private readonly IAzureScrapingSystemMetricsPublisher _azureScrapingSystemMetricsPublisher;
        public override string DependencyName => "Azure Resource Manager (ARM)";
        private const string ThrottlingHeaderName = "x-ms-ratelimit-remaining-subscription-reads";
        private const string AvailableCallsMetricDescription = "Indication how many calls are still available before Azure Resource Manager (ARM) is going to throttle us.";
        private const string ThrottledMetricDescription = "Indication concerning Azure Resource Manager are being throttled. (1 = yes, 0 = no).";

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="tenantId">Id of the tenant that is being interacted with via Azure Resource Manager</param>
        /// <param name="subscriptionId">Id of the subscription that is being interacted with via Azure Resource Manager</param>
        /// <param name="azureAuthenticationInfo">Information regarding authentication with Microsoft Azure</param>
        /// <param name="metricSinkWriter">Metrics writer to all sinks</param>
        /// <param name="azureScrapingSystemMetricsPublisher">Metrics collector to write metrics to Prometheus</param>
        /// <param name="logger">Logger to write telemetry to</param>
        public AzureResourceManagerThrottlingRequestHandler(string tenantId, string subscriptionId, AzureAuthenticationInfo azureAuthenticationInfo, MetricSinkWriter metricSinkWriter, IAzureScrapingSystemMetricsPublisher azureScrapingSystemMetricsPublisher, ILogger logger)
            : base(azureScrapingSystemMetricsPublisher, logger)
        {
            Guard.NotNullOrWhitespace(tenantId, nameof(tenantId));
            Guard.NotNullOrWhitespace(subscriptionId, nameof(subscriptionId));
            Guard.NotNull(metricSinkWriter, nameof(metricSinkWriter));
            Guard.NotNull(azureScrapingSystemMetricsPublisher, nameof(azureScrapingSystemMetricsPublisher));
            Guard.NotNull(azureAuthenticationInfo, nameof(azureAuthenticationInfo));

            _metricSinkWriter = metricSinkWriter;
            _azureScrapingSystemMetricsPublisher = azureScrapingSystemMetricsPublisher;

            var id = DetermineApplicationId(azureAuthenticationInfo);
  
            _metricLabels = new Dictionary<string, string>
            {
                {"tenant_id", tenantId},
                {"subscription_id", subscriptionId},
                {"app_id", id},
            };
        }

        protected override HttpRequestMessage BeforeSendingRequest(HttpRequestMessage request)
        {
            string agentVersion = Version.Get();
            var promitorUserAgent = UserAgent.Generate("Scraper", agentVersion);
            request.Headers.UserAgent.Clear();
            request.Headers.UserAgent.TryParseAdd(promitorUserAgent);

            return request;
        }
        
        protected override async Task AvailableRateLimitingCallsAsync(HttpResponseMessage response)
        {
            // Source: https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-request-limits
            if (response.Headers.Contains(ThrottlingHeaderName))
            {
                var remainingApiCalls = response.Headers.GetValues(ThrottlingHeaderName).FirstOrDefault();
                var subscriptionReadLimit = Convert.ToInt16(remainingApiCalls);
                
                // Report metric
                await _metricSinkWriter.ReportMetricAsync(RuntimeMetricNames.RateLimitingForArm, AvailableCallsMetricDescription, subscriptionReadLimit, _metricLabels);
                await _azureScrapingSystemMetricsPublisher.WriteGaugeMeasurementAsync(RuntimeMetricNames.RateLimitingForArm, AvailableCallsMetricDescription, subscriptionReadLimit, _metricLabels);
            }
        }

        protected override Dictionary<string, string> GetMetricLabels() => _metricLabels;
        protected override string GetThrottlingStatusMetricName() => RuntimeMetricNames.ArmThrottled;
        protected override string GetThrottlingStatusMetricDescription() => ThrottledMetricDescription;

        private string DetermineApplicationId(AzureAuthenticationInfo azureAuthenticationInfo)
        {
            switch (azureAuthenticationInfo.Mode)
            {
                case AuthenticationMode.ServicePrincipal:
                case AuthenticationMode.UserAssignedManagedIdentity:
                    Guard.NotNullOrWhitespace(azureAuthenticationInfo.IdentityId, nameof(azureAuthenticationInfo.IdentityId));
                    return azureAuthenticationInfo.IdentityId;
                case AuthenticationMode.SystemAssignedManagedIdentity:
                    return "system-assigned-identity";
                default:
                    throw new ArgumentOutOfRangeException(nameof(azureAuthenticationInfo.Mode));
            }
        }
    }
}