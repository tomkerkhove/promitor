using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using System.Collections.Generic;
using System.Linq;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Scraper.Host.Validation.MetricDefinitions.Interfaces;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class RedisCacheMetricValidator : IMetricValidator
    {
        public IEnumerable<string> Validate(MetricDefinition metricDefinition)
        {
            Guard.NotNull(metricDefinition, nameof(metricDefinition));

            foreach (var resourceDefinition in metricDefinition.Resources.Cast<RedisCacheResourceDefinition>())
            {
                if (string.IsNullOrWhiteSpace(resourceDefinition.CacheName))
                {
                    yield return "No cache name is configured";
                }
            }
        }
    }
}
