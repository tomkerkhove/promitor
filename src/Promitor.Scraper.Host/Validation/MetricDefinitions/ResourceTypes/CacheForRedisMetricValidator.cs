using GuardNet;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using System.Collections.Generic;

namespace Promitor.Scraper.Host.Validation.MetricDefinitions.ResourceTypes
{
    internal class CacheForRedisMetricValidator : MetricValidator<CacheForRedisMetricDefinition>
    {
        protected override IEnumerable<string> Validate(CacheForRedisMetricDefinition cacheForRedisMetricDefinition)
        {
            Guard.NotNull(cacheForRedisMetricDefinition, nameof(cacheForRedisMetricDefinition));

            if (string.IsNullOrWhiteSpace(cacheForRedisMetricDefinition.CacheName))
            {
                yield return "No cache name is configured";
            }
        }
    }
}
