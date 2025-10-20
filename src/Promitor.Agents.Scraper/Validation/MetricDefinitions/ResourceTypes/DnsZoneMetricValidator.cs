using System.Collections.Generic;
using System.Linq;
using GuardNet;
using Promitor.Agents.Scraper.Validation.MetricDefinitions.Interfaces;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Agents.Scraper.Validation.MetricDefinitions.ResourceTypes
{
    internal class DnsZoneMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<DnsZoneResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.ZoneName))
                {
                    yield return "No DNS Zone name is configured";
                }
            }
        }
    }
}