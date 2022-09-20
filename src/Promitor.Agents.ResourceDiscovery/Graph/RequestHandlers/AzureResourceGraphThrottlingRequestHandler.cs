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

namespace Promitor.Agents.ResourceDiscovery.Graph.RequestHandlers
{
    internal class AzureResourceGraphThrottlingRequestHandler : ThrottlingRequestHandler
    {
        private readonly Dictionary<string, string> _metricLabels;

        private const string ThrottlingHeaderName = "x-ms-user-quota-remaining";
        private const string AvailableCallsMetricDescription = "Indication how many calls are still available before Azure Resource Graph is going to throttle us.";
        private const string ThrottledMetricDescription = "Indication concerning Azure Resource Graph are being throttled. (1 = yes, 0 = no).";

        public override string DependencyName => "Azure Resource Graph";

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="systemMetricsPublisher">Metrics collector to write metrics to Prometheus</param>
        /// <param name="metricLabels"></param>
        /// <param name="logger">Logger to write telemetry to</param>
        public AzureResourceGraphThrottlingRequestHandler(ISystemMetricsPublisher systemMetricsPublisher, Dictionary<string, string> metricLabels, ILogger logger)
        : base(systemMetricsPublisher, logger)
        {
            Guard.NotNull(metricLabels, nameof(metricLabels));

            _metricLabels = metricLabels;
        }

        protected override async Task AvailableRateLimitingCallsAsync(HttpResponseMessage response)
        {
            // Source:
            // - https://docs.microsoft.com/en-us/azure/governance/resource-graph/overview#throttling
            // - https://docs.microsoft.com/en-us/azure/governance/resource-graph/concepts/guidance-for-throttled-requests#understand-throttling-headers
            if (response.Headers.Contains(ThrottlingHeaderName))
            {
                var remainingApiCalls = response.Headers.GetValues(ThrottlingHeaderName).FirstOrDefault();
                var subscriptionReadLimit = Convert.ToInt16(remainingApiCalls);

                // Report metric
                await SystemMetricsPublisher.WriteGaugeMeasurementAsync(RuntimeMetricNames.RateLimitingForResourceGraph, AvailableCallsMetricDescription, subscriptionReadLimit, _metricLabels, includeTimestamp: true);
            }
        }

        protected override Dictionary<string, string> GetMetricLabels() => _metricLabels;
        protected override string GetThrottlingStatusMetricName() => RuntimeMetricNames.ResourceGraphThrottled;
        protected override string GetThrottlingStatusMetricDescription() => ThrottledMetricDescription;
    }
}
