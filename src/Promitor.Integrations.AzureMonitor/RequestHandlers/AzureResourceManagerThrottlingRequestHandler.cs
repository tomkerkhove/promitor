using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Extensions.Logging;
using Promitor.Core;
using Promitor.Core.Metrics;
using Promitor.Core.Metrics.Sinks;

namespace Promitor.Integrations.AzureMonitor.RequestHandlers
{
    /// <summary>
    ///     Request handler to provide insights on the current api consumption and throttling of Azure Resource Manager
    /// </summary>
    public class AzureResourceManagerThrottlingRequestHandler : DelegatingHandlerBase
    {
        private readonly ILogger _logger;
        private readonly Dictionary<string, string> _metricLabels;
        private readonly MetricSinkWriter _metricSinkWriter;
        private readonly IRuntimeMetricsCollector _metricsCollector;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="tenantId">Id of the tenant that is being interacted with via Azure Resource Manager</param>
        /// <param name="subscriptionId">Id of the subscription that is being interacted with via Azure Resource Manager</param>
        /// <param name="applicationId">Id of the application that is being used to interact with Azure Resource Manager</param>
        /// <param name="metricSinkWriter">Metrics writer to all sinks</param>
        /// <param name="metricsCollector">Metrics collector to write metrics to Prometheus</param>
        /// <param name="logger">Logger to write telemetry to</param>
        public AzureResourceManagerThrottlingRequestHandler(string tenantId, string subscriptionId, string applicationId, MetricSinkWriter metricSinkWriter, IRuntimeMetricsCollector metricsCollector, ILogger logger)
        {
            Guard.NotNullOrWhitespace(tenantId, nameof(tenantId));
            Guard.NotNullOrWhitespace(subscriptionId, nameof(subscriptionId));
            Guard.NotNullOrWhitespace(applicationId, nameof(applicationId));
            Guard.NotNull(metricSinkWriter, nameof(metricSinkWriter));
            Guard.NotNull(metricsCollector, nameof(metricsCollector));
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;
            _metricSinkWriter = metricSinkWriter;
            _metricsCollector = metricsCollector;

            _metricLabels = new Dictionary<string, string>
            {
                {"tenant_id", tenantId},
                {"subscription_id", subscriptionId},
                {"app_id", applicationId}
            };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            await MeasureArmRateLimitingAsync(response);

            if ((int)response.StatusCode == 429)
            {
                LogArmThrottling();
            }

            return response;
        }

        private async Task MeasureArmRateLimitingAsync(HttpResponseMessage response)
        {
            // Source: https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-request-limits
            if (response.Headers.Contains("x-ms-ratelimit-remaining-subscription-reads"))
            {
                var remainingApiCalls = response.Headers.GetValues("x-ms-ratelimit-remaining-subscription-reads").FirstOrDefault();
                var subscriptionReadLimit = Convert.ToInt16(remainingApiCalls);
                await _metricSinkWriter.ReportMetricAsync(RuntimeMetricNames.RateLimitingForArm, "Indication how many calls are still available before Azure Resource Manager is going to throttle us.", subscriptionReadLimit, _metricLabels);
                _metricsCollector.SetGaugeMeasurement(RuntimeMetricNames.RateLimitingForArm, "Indication how many calls are still available before Azure Resource Manager is going to throttle us.", subscriptionReadLimit, _metricLabels);
            }
        }

        private void LogArmThrottling()
        {
            _logger.LogWarning("Azure subscription rate limit reached.");
        }
    }
}