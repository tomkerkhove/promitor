using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using GuardNet;
using Promitor.Core;
using Promitor.Core.Metrics.Interfaces;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.Azure.Authentication;

namespace Promitor.Integrations.AzureMonitor.HttpPipelinePolicies{
    public class RecordArmRateLimitMetricsPolicy : HttpPipelinePolicy
    {   
        private readonly Dictionary<string, string> _metricLabels;
        private readonly MetricSinkWriter _metricSinkWriter;
        private readonly IAzureScrapingSystemMetricsPublisher _azureScrapingSystemMetricsPublisher;
        private const string ThrottlingHeaderName = "x-ms-ratelimit-remaining-subscription-reads";
        private const string AvailableCallsMetricDescription = "Indication how many calls are still available before Azure Resource Manager (ARM) is going to throttle us.";
        private const string ThrottledMetricDescription = "Indication concerning Azure Resource Manager are being throttled. (1 = yes, 0 = no).";

        public RecordArmRateLimitMetricsPolicy(string tenantId, string subscriptionId, AzureAuthenticationInfo azureAuthenticationInfo, MetricSinkWriter metricSinkWriter, IAzureScrapingSystemMetricsPublisher azureScrapingSystemMetricsPublisher)
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

        public override async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {   
            await ProcessNextAsync(message, pipeline);
            // Source: https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-request-limits
            if (message.Response.Headers.Contains(ThrottlingHeaderName))
            {   
                message.Response.Headers.TryGetValue(ThrottlingHeaderName, out string remainingApiCallsStr);
                var subscriptionReadLimit = Convert.ToInt16(remainingApiCallsStr);
                
                // Report metric
                await _metricSinkWriter.ReportMetricAsync(RuntimeMetricNames.RateLimitingForArm, AvailableCallsMetricDescription, subscriptionReadLimit, _metricLabels);
                await _azureScrapingSystemMetricsPublisher.WriteGaugeMeasurementAsync(RuntimeMetricNames.RateLimitingForArm, AvailableCallsMetricDescription, subscriptionReadLimit, _metricLabels);
            }

            // throttled by ARM
            await _azureScrapingSystemMetricsPublisher.WriteGaugeMeasurementAsync(RuntimeMetricNames.ArmThrottled, ThrottledMetricDescription, message.Response.Status == 429 ? 1 : 0, _metricLabels, includeTimestamp: true);
        }

        public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            throw new NotSupportedException("Synchronous HTTP path is not supported");
        }

        private string DetermineApplicationId(AzureAuthenticationInfo azureAuthenticationInfo)
        {
            switch (azureAuthenticationInfo.Mode)
            {
                case AuthenticationMode.ServicePrincipal:
                    Guard.NotNullOrWhitespace(azureAuthenticationInfo.IdentityId, nameof(azureAuthenticationInfo.IdentityId));
                    return azureAuthenticationInfo.IdentityId;
                case AuthenticationMode.UserAssignedManagedIdentity:
                    return azureAuthenticationInfo.GetIdentityIdOrDefault("externally-configured-user-assigned-identity");
                case AuthenticationMode.SystemAssignedManagedIdentity:
                    return "system-assigned-identity";
                case AuthenticationMode.SdkDefault:
                    return "default-azure-credentials";                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(azureAuthenticationInfo.Mode));
            }
        }
    }
}   