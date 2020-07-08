using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core;
using Promitor.Core.Metrics.Sinks;

namespace Promitor.Agents.Scraper.Temporary
{
    public class AtlassianStatuspageMetricSink : IMetricSink
    {
        private readonly ILogger<AtlassianStatuspageMetricSink> _logger;
        private readonly AtlassianStatuspage _atlassianStatuspage;
        public MetricSinkType Type { get; } = MetricSinkType.AtlassianStatuspage;

        public AtlassianStatuspageMetricSink(AtlassianStatuspage atlassianStatuspage, ILogger<AtlassianStatuspageMetricSink> logger)
        {
            Guard.NotNull(atlassianStatuspage, nameof(atlassianStatuspage));
            Guard.NotNull(logger, nameof(logger));

            _atlassianStatuspage = atlassianStatuspage;
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

            if(metricName.Contains("arm")==false)
            {
                await _atlassianStatuspage.ReportMetricAsync("nfkgnrwpn545", metricValue, "53dbaf1c-8551-48ef-81f9-481eb3ac64cb");
            }
            _logger.LogTrace("Metric {MetricName} with value {MetricValue} was written to Atlassian Statuspage", metricName, metricValue);
        }
    }
}
