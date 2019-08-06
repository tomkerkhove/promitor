using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using System.Collections.Generic;
using System.Linq;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class PostgreSqlMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<PostgreSqlMetricDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.ServerName))
                {
                    yield return "No server name is configured";
                }
            }
        }
    }
}
