using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    /// <summary>
    /// Validates <see cref="AzureSqlDatabaseResourceDefinition" /> objects.
    /// </summary>
    public class AzureSqlDatabaseMetricValidator : IMetricValidator
    {
        /// <inheritdoc />
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var definition in metricDefinition.Resources.Cast<AzureSqlDatabaseResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(definition.ServerName))
                {
                    yield return "No server name is configured";
                }

                if (string.IsNullOrWhiteSpace(definition.DatabaseName))
                {
                    yield return "No database name is configured";
                }
            }
        }
    }
}