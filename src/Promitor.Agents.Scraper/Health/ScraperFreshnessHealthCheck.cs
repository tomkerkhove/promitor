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
        private readonly ILastSuccessfulScrapeStore _lastSuccessfulScrapeStore;
        private readonly ILogger<ScraperResultFreshnessHealthCheck> _logger;
        private readonly TimeSpan _unhealthyThreshold;
        public ScraperResultFreshnessHealthCheck(
            ILastSuccessfulScrapeStore lastSuccessfulScrapeStore,
            IScrapeScheduleProvider scrapeScheduleProvider,
            ILogger<ScraperResultFreshnessHealthCheck> logger)
        {
            Guard.NotNull(lastSuccessfulScrapeStore, nameof(lastSuccessfulScrapeStore));
            Guard.NotNull(scrapeScheduleProvider, nameof(scrapeScheduleProvider));
            Guard.NotNull(logger, nameof(logger));

            _lastSuccessfulScrapeStore = lastSuccessfulScrapeStore;
            _logger = logger;

            // Calculate threshold once during initialization
            var _minimumInterval = scrapeScheduleProvider.GetMinimumScrapeInterval();
            _unhealthyThreshold = TimeSpan.FromTicks(_minimumInterval.Ticks * 2);

            _logger.LogInformation("Health check unhealthy threshold calculated as {ThresholdSeconds}s (2x the minimum scrape interval of {MinimumIntervalSeconds}s)",
                _unhealthyThreshold.TotalSeconds, _minimumInterval.TotalSeconds);
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
            _logger.LogInformation("Last successful scrape at {LastScrapeUtc:o}; age {AgeSeconds}s; threshold {ThresholdSeconds}s (2x {MinimumInterval}s interval).",
                last.Value, age.TotalSeconds, _unhealthyThreshold.TotalSeconds, _minimumInterval.TotalSeconds);

            if (age > _unhealthyThreshold)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"Last successful scrape was {age.TotalSeconds:F0}s ago, exceeding threshold of {_unhealthyThreshold.TotalSeconds:F0}s (2x the minimum scrape interval of {_minimumInterval.TotalSeconds:F0}s)."));
            }

            return Task.FromResult(HealthCheckResult.Healthy($"Last successful scrape {age.TotalSeconds:F0}s ago."));
        }
    }
}


