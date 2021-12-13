﻿using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.ResourceTypes
{
    public class RedisEnterpriseCacheScraper : AzureMonitorScraper<RedisEnterpriseCacheResourceDefinition>
    {
        private const string ResourceUriTemplate = "subscriptions/{0}/resourceGroups/{1}/providers/Microsoft.Cache/RedisEnterprise/{2}";

        public RedisEnterpriseCacheScraper(ScraperConfiguration scraperConfiguration)
           : base(scraperConfiguration)
        {
        }

        protected override string BuildResourceUri(string subscriptionId, ScrapeDefinition<IAzureResourceDefinition> scrapeDefinition, RedisEnterpriseCacheResourceDefinition resource)
        {
           return string.Format(ResourceUriTemplate, subscriptionId, scrapeDefinition.ResourceGroupName, resource.CacheName);
        }
    }
}
