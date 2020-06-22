using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    /// <summary>
    /// Validates <see cref="SqlManagedInstanceMetricValidator" /> objects.
    /// </summary>
    public class SqlManagedInstanceMetricValidator : IMetricValidator
    {
        /// <inheritdoc />
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var definition in metricDefinition.Resources.Cast<SqlManagedInstanceResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(definition.InstanceName))
                {
                    yield return "No instance name is configured";
                }
            }
        }
    }
}