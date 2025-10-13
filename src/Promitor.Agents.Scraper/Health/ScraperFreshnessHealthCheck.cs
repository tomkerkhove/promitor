using System;
using System.Threading;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Promitor.Agents.Scraper.Runtime;

namespace Promitor.Agents.Scraper.Health
{
    public class ScraperResultFreshnessHealthCheck : IHealthCheck
    {
        private static readonly TimeSpan UnhealthyThreshold = TimeSpan.FromMinutes(5);
        private readonly ILastSuccessfulScrapeStore _lastSuccessfulScrapeStore;

        public ScraperResultFreshnessHealthCheck(ILastSuccessfulScrapeStore lastSuccessfulScrapeStore)
        {
            Guard.NotNull(lastSuccessfulScrapeStore, nameof(lastSuccessfulScrapeStore));
            _lastSuccessfulScrapeStore = lastSuccessfulScrapeStore;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            var last = _lastSuccessfulScrapeStore.GetLast();
            if (last == null)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("No successful scrape has been recorded yet."));
            }

            var age = DateTimeOffset.UtcNow - last.Value;
            if (age > UnhealthyThreshold)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"Last successful scrape was {age.TotalSeconds:F0}s ago, exceeding threshold of {UnhealthyThreshold.TotalSeconds:F0}s."));
            }

            return Task.FromResult(HealthCheckResult.Healthy($"Last successful scrape {age.TotalSeconds:F0}s ago."));
        }
    }
}


