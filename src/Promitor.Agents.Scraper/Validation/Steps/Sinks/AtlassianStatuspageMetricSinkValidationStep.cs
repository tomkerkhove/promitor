using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Agents.Scraper.Configuration;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;

namespace Promitor.Agents.Scraper.Validation.Steps.Sinks
{
    public class AtlassianStatuspageMetricSinkValidationStep : ValidationStep,
        IValidationStep
    {
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly IOptions<ScraperRuntimeConfiguration> _runtimeConfiguration;

        public AtlassianStatuspageMetricSinkValidationStep(IOptions<ScraperRuntimeConfiguration> runtimeConfiguration, IMetricsDeclarationProvider metricsDeclarationProvider, ILogger<AtlassianStatuspageMetricSinkValidationStep> validationLogger)
            : base(validationLogger)
        {
            _runtimeConfiguration = runtimeConfiguration;
            _metricsDeclarationProvider = metricsDeclarationProvider;
        }

        public string ComponentName { get; } = "Atlassian Statuspage Metric Sink";

        public ValidationResult Run()
        {
            var currentRuntimeConfiguration = _runtimeConfiguration.Value;
            var atlassianStatuspageConfiguration = currentRuntimeConfiguration.MetricSinks?.AtlassianStatuspage;
            if (atlassianStatuspageConfiguration == null)
            {
                return ValidationResult.Successful(ComponentName);
            }

            var errorMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(atlassianStatuspageConfiguration.PageId))
            {
                errorMessages.Add("No page id of Atlassian Status page is configured");
            }

            if (atlassianStatuspageConfiguration.SystemMetricMapping?.Any() != true)
            {
                errorMessages.Add("No system metrics mappings are configured which means no metrics can be reported");
            }
            else
            {
                if (atlassianStatuspageConfiguration.SystemMetricMapping.Select(map => map.Id).Distinct().Count() != atlassianStatuspageConfiguration.SystemMetricMapping?.Count)
                {
                    errorMessages.Add("System metric with duplicate id(s) mappings are configured");
                }

                var metricsDeclaration = _metricsDeclarationProvider.Get(true);

                if (atlassianStatuspageConfiguration.SystemMetricMapping == null)
                {
                    return errorMessages.Any() ? ValidationResult.Failure(ComponentName, errorMessages) : ValidationResult.Successful(ComponentName);
                }

                foreach (var systemMetric in atlassianStatuspageConfiguration.SystemMetricMapping)
                {
                    if (string.IsNullOrWhiteSpace(systemMetric.Id))
                    {
                        errorMessages.Add($"System metric mapping defined without specifying a system metric id (Promitor metric name: {systemMetric.PromitorMetricName})");
                    }
                    if (string.IsNullOrWhiteSpace(systemMetric.PromitorMetricName))
                    {
                        errorMessages.Add($"System metric mapping defined without specifying a Promitor metric name (System metric id: {systemMetric.Id})");
                    }

                    var matchingPromitorMetric = metricsDeclaration.Metrics.FirstOrDefault(metricDefinition => metricDefinition.PrometheusMetricDefinition.Name.Equals(systemMetric.PromitorMetricName));
                    if (matchingPromitorMetric == null)
                    {
                        errorMessages.Add($"Statuspage metric Id '{systemMetric.Id}' is mapped to a metric called '{systemMetric.PromitorMetricName}', but no metric was found with that name");
                    }
                    else
                    {
                        if (matchingPromitorMetric.ResourceDiscoveryGroups?.Any() == true)
                        {
                            errorMessages.Add("Scraping with resource discovery is not supported");
                        }

                        if (matchingPromitorMetric.Resources?.Count > 1)
                        {
                            errorMessages.Add("Scraping multiple resources for one metric is not supported");
                        }
                    }
                }
            }

            return errorMessages.Any() ? ValidationResult.Failure(ComponentName, errorMessages) : ValidationResult.Successful(ComponentName);
        }
    }
}
