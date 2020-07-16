﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Core;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.Sinks.Atlassian.Statuspage.Configuration;

namespace Promitor.Integrations.Sinks.Atlassian.Statuspage
{
    public class AtlassianStatuspageMetricSink : IMetricSink
    {
        private readonly ILogger<AtlassianStatuspageMetricSink> _logger;
        private readonly IAtlassianStatuspageClient _atlassianStatusPageClient;
        private readonly IOptionsMonitor<AtlassianStatusPageSinkConfiguration> _sinkConfiguration;

        public MetricSinkType Type { get; } = MetricSinkType.AtlassianStatuspage;

        public AtlassianStatuspageMetricSink(IAtlassianStatuspageClient atlassianStatusPageClient, IOptionsMonitor<AtlassianStatusPageSinkConfiguration> sinkConfiguration, ILogger<AtlassianStatuspageMetricSink> logger)
        {
            Guard.NotNull(atlassianStatusPageClient, nameof(atlassianStatusPageClient));
            Guard.NotNull(sinkConfiguration, nameof(sinkConfiguration));
            Guard.NotNull(sinkConfiguration.CurrentValue, nameof(sinkConfiguration.CurrentValue));
            Guard.NotNull(logger, nameof(logger));

            _atlassianStatusPageClient = atlassianStatusPageClient;
            _sinkConfiguration = sinkConfiguration;
            _logger = logger;
        }

        public async Task ReportMetricAsync(string metricName, string metricDescription, ScrapeResult scrapeResult)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));
            Guard.NotNull(scrapeResult, nameof(scrapeResult));
            Guard.NotNull(scrapeResult.MetricValues, nameof(scrapeResult.MetricValues));

            var reportMetricTasks = new List<Task>();

            foreach (var measuredMetric in scrapeResult.MetricValues)
            {
                var metricValue = measuredMetric.Value ?? 0;

                var reportMetricTask = ReportMetricAsync(metricName, metricDescription, metricValue, new Dictionary<string, string>());
                reportMetricTasks.Add(reportMetricTask);
            }

            await Task.WhenAll(reportMetricTasks);
        }

        public async Task ReportMetricAsync(string metricName, string metricDescription, double metricValue, Dictionary<string, string> labels)
        {
            Guard.NotNullOrEmpty(metricName, nameof(metricName));

            var systemMetricMapping = _sinkConfiguration.CurrentValue.SystemMetricMapping.SingleOrDefault(metricMapping => metricMapping.PromitorMetricName.Equals(metricName));
            if(systemMetricMapping != null)
            {
                await _atlassianStatusPageClient.ReportMetricAsync(systemMetricMapping.Id, metricValue);
            }

            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was written to Atlassian Statuspage", metricName, metricValue);
        }
    }
}
