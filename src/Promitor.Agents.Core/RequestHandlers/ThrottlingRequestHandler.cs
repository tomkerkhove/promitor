using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Metrics.Prometheus.Collectors.Interfaces;

namespace Promitor.Agents.Core.RequestHandlers
{
    public abstract class ThrottlingRequestHandler : DelegatingHandler
    {
        public abstract string DependencyName { get; }

        protected ILogger Logger { get; }
        protected ISystemMetricsCollector SystemMetricsCollector { get; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="systemMetricsCollector">Metrics collector for Prometheus</param>
        /// <param name="logger">Logger to write telemetry to</param>
        protected ThrottlingRequestHandler(ISystemMetricsCollector systemMetricsCollector, ILogger logger)
        {
            Guard.NotNull(systemMetricsCollector, nameof(systemMetricsCollector));
            Guard.NotNull(logger, nameof(logger));

            Logger = logger;
            SystemMetricsCollector = systemMetricsCollector;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request = BeforeSendingRequest(request);

            var response = await base.SendAsync(request, cancellationToken);

            var wasRequestThrottled = (int)response.StatusCode == 429;
            if (wasRequestThrottled)
            {
                LogArmThrottling();
            }

            await AvailableRateLimitingCallsAsync(response);
            AvailableThrottlingStatusAsync(wasRequestThrottled);
            
            return response;
        }

        private void AvailableThrottlingStatusAsync(bool wasRequestThrottled)
        {
            var metricValue = wasRequestThrottled ? 1 : 0;
            var metricLabels = GetMetricLabels();
            SystemMetricsCollector.WriteGaugeMeasurement(GetThrottlingStatusMetricName(), GetThrottlingStatusMetricDescription(), metricValue, metricLabels, includeTimestamp: true);
        }

        protected abstract Dictionary<string, string> GetMetricLabels();
        protected abstract string GetThrottlingStatusMetricName();
        protected abstract string GetThrottlingStatusMetricDescription();
        protected abstract Task AvailableRateLimitingCallsAsync(HttpResponseMessage response);

        protected virtual HttpRequestMessage BeforeSendingRequest(HttpRequestMessage request)
        {
            return request;
        }

        protected void LogArmThrottling()
        {
            Logger.LogWarning($"{DependencyName} rate limit reached.");
        }
    }
}
