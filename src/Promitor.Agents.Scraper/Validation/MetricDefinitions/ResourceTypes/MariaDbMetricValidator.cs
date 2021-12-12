using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    /// <summary>
    /// Validates <see cref="MariaDbMetricValidator" /> objects.
    /// </summary>
    public class MariaDbMetricValidator : IMetricValidator
    {
        /// <inheritdoc />
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var definition in metricDefinition.Resources.Cast<MariaDbResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(definition.ServerName))
                {
                    yield return "No server name is configured";
                }
            }
        }
    }
}