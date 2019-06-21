using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using System.Collections.Generic;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class PostgreSqlMetricValidator : MetricValidator<PostgreSqlMetricDefinition>
    {
        protected override IEnumerable<string> Validate(PostgreSqlMetricDefinition postgreSqlMetricDefinition)
        {
            Guard.NotNull(postgreSqlMetricDefinition, nameof(postgreSqlMetricDefinition));

            if (string.IsNullOrWhiteSpace(postgreSqlMetricDefinition.ServerName))
            {
                yield return "No server name is configured";
            }
        }
    }
}
