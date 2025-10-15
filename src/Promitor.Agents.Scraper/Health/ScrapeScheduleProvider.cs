using System;
using System.Linq;
using Cronos;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;

namespace Promitor.Agents.Scraper.Health
{
    /// <summary>
    /// Provides information about the configured scrape schedules
    /// </summary>
    public class ScrapeScheduleProvider : IScrapeScheduleProvider
    {
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        private readonly ILogger<ScrapeScheduleProvider> _logger;
        private TimeSpan? _cachedMinimumInterval;

        public ScrapeScheduleProvider(IMetricsDeclarationProvider metricsDeclarationProvider, ILogger<ScrapeScheduleProvider> logger)
        {
            Guard.NotNull(metricsDeclarationProvider, nameof(metricsDeclarationProvider));
            Guard.NotNull(logger, nameof(logger));

            _metricsDeclarationProvider = metricsDeclarationProvider;
            _logger = logger;
        }

        /// <summary>
        /// Gets the minimum scrape interval across all configured metrics
        /// </summary>
        public TimeSpan GetMinimumScrapeInterval()
        {
            if (_cachedMinimumInterval.HasValue)
            {
                return _cachedMinimumInterval.Value;
            }

            var metricsDeclaration = _metricsDeclarationProvider.Get(applyDefaults: true);
            if (metricsDeclaration?.Metrics == null || !metricsDeclaration.Metrics.Any())
            {
                _logger.LogWarning("No metrics configured, using default interval of 5 minutes");
                return TimeSpan.FromMinutes(5);
            }

            var intervals = metricsDeclaration.Metrics
                .Select(m => m.Scraping?.Schedule)
                .Where(schedule => !string.IsNullOrWhiteSpace(schedule))
                .Distinct()
                .Select(schedule => CalculateIntervalFromCronExpression(schedule))
                .Where(interval => interval.HasValue)
                .Select(interval => interval.Value)
                .ToList();

            if (!intervals.Any())
            {
                _logger.LogWarning("Unable to calculate intervals from cron expressions, using default interval of 5 minutes");
                return TimeSpan.FromMinutes(5);
            }

            _cachedMinimumInterval = intervals.Min();
            _logger.LogInformation("Calculated minimum scrape interval: {MinimumInterval}", _cachedMinimumInterval.Value);

            return _cachedMinimumInterval.Value;
        }

        private TimeSpan? CalculateIntervalFromCronExpression(string cronExpression)
        {
            try
            {
                var cron = CronExpression.Parse(cronExpression);
                var baseTime = DateTimeOffset.UtcNow;
                var next1 = cron.GetNextOccurrence(baseTime, TimeZoneInfo.Utc);

                if (!next1.HasValue)
                {
                    _logger.LogWarning("Unable to calculate next occurrence for cron expression: {CronExpression}", cronExpression);
                    return null;
                }

                var next2 = cron.GetNextOccurrence(next1.Value, TimeZoneInfo.Utc);

                if (!next2.HasValue)
                {
                    _logger.LogWarning("Unable to calculate second occurrence for cron expression: {CronExpression}", cronExpression);
                    return null;
                }

                var interval = next2.Value - next1.Value;
                _logger.LogDebug("Calculated interval {Interval} for cron expression: {CronExpression}", interval, cronExpression);

                return interval;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse cron expression: {CronExpression}", cronExpression);
                return null;
            }
        }
    }
}

