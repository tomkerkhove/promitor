using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Metrics.Interfaces;

namespace Promitor.Agents.Core.RequestHandlers
{
    public abstract class ThrottlingRequestHandler : DelegatingHandler
    {
        public abstract string DependencyName { get; }

        protected ILogger Logger { get; }
        protected ISystemMetricsPublisher SystemMetricsPublisher { get; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="systemMetricsPublisher">Metrics collector for Prometheus</param>
        /// <param name="logger">Logger to write telemetry to</param>
        protected ThrottlingRequestHandler(ISystemMetricsPublisher systemMetricsPublisher, ILogger logger)
        {
            Guard.NotNull(systemMetricsPublisher, nameof(systemMetricsPublisher));
            Guard.NotNull(logger, nameof(logger));

            Logger = logger;
            SystemMetricsPublisher = systemMetricsPublisher;
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
            await AvailableThrottlingStatusAsync(wasRequestThrottled);
            
            return response;
        }

        private async Task AvailableThrottlingStatusAsync(bool wasRequestThrottled)
        {
            var metricValue = wasRequestThrottled ? 1 : 0;
            var metricLabels = GetMetricLabels();
            await SystemMetricsPublisher.WriteGaugeMeasurementAsync(GetThrottlingStatusMetricName(), GetThrottlingStatusMetricDescription(), metricValue, metricLabels, includeTimestamp: true);
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
