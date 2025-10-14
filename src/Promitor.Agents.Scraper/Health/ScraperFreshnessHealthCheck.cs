using System;
using System.Threading;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Scraper.Runtime;

namespace Promitor.Agents.Scraper.Health
{
    public class ScraperResultFreshnessHealthCheck : IHealthCheck
    {
        private static readonly TimeSpan UnhealthyThreshold = TimeSpan.FromMinutes(5);
        private readonly ILastSuccessfulScrapeStore _lastSuccessfulScrapeStore;
        private readonly ILogger<ScraperResultFreshnessHealthCheck> _logger;

        public ScraperResultFreshnessHealthCheck(ILastSuccessfulScrapeStore lastSuccessfulScrapeStore, ILogger<ScraperResultFreshnessHealthCheck> logger)
        {
            Guard.NotNull(lastSuccessfulScrapeStore, nameof(lastSuccessfulScrapeStore));
            Guard.NotNull(logger, nameof(logger));
            _lastSuccessfulScrapeStore = lastSuccessfulScrapeStore;
            _logger = logger;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            var last = _lastSuccessfulScrapeStore.GetLast();
            if (last == null)
            {
                _logger.LogInformation("No successful scrape recorded yet.");
                return Task.FromResult(HealthCheckResult.Unhealthy("No successful scrape has been recorded yet."));
            }

            var age = DateTimeOffset.UtcNow - last.Value;
            _logger.LogInformation("Last successful scrape at {LastScrapeUtc:o}; age {AgeSeconds}s; threshold {ThresholdSeconds}s.", last.Value, age.TotalSeconds, UnhealthyThreshold.TotalSeconds);
            if (age > UnhealthyThreshold)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"Last successful scrape was {age.TotalSeconds:F0}s ago, exceeding threshold of {UnhealthyThreshold.TotalSeconds:F0}s."));
            }

            return Task.FromResult(HealthCheckResult.Healthy($"Last successful scrape {age.TotalSeconds:F0}s ago."));
        }
    }
}


