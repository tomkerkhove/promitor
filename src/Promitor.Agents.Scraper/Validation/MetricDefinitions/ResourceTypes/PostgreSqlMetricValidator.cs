using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Contracts.ResourceTypes.Enums;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class PostgreSqlMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<PostgreSqlResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.ServerName))
                {
                    yield return "No server name is configured";
                }

                if (resourceDefinition.Type == PostgreSqlServerType.Arc)
                {
                    yield return "Scraping Azure Arc-based servers is not supported yet";
                }
            }
        }
    }
}
