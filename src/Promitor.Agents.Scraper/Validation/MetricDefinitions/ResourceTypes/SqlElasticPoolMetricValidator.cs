using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    /// <summary>
    /// Validates <see cref="SqlElasticPoolMetricValidator" /> objects.
    /// </summary>
    public class SqlElasticPoolMetricValidator : IMetricValidator
    {
        /// <inheritdoc />
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var definition in metricDefinition.Resources.Cast<SqlElasticPoolResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(definition.ServerName))
                {
                    yield return "No server name is configured";
                }

                if (string.IsNullOrWhiteSpace(definition.PoolName))
                {
                    yield return "No pool name is configured";
                }
            }
        }
    }
}