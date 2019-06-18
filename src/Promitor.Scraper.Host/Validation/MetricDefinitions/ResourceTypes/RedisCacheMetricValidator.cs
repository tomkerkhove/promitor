using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using System.Collections.Generic;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class RedisCacheMetricValidator : MetricValidator<RedisCacheMetricDefinition>
    {
        protected override IEnumerable<string> Validate(RedisCacheMetricDefinition redisCacheMetricDefinition)
        {
            Guard.NotNull(redisCacheMetricDefinition, nameof(redisCacheMetricDefinition));

            if (string.IsNullOrWhiteSpace(redisCacheMetricDefinition.CacheName))
            {
                yield return "No cache name is configured";
            }
        }
    }
}
