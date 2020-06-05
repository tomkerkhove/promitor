using System;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Scraper.Discovery;

namespace Promitor.Agents.Scraper.Scheduling
{
    public class ResourceCollectionScrapingJob : MetricScrapingJob, IScheduledJob
    {
        private readonly ResourceDiscoveryRepository _resourceDiscoveryRepository;

        public ResourceCollectionScrapingJob(string name, string resourceCollectionName, ResourceDiscoveryRepository resourceDiscoveryRepository, ILogger<ResourceCollectionScrapingJob> logger)
            : base(name, logger)
        {
            Guard.NotNullOrWhitespace(resourceCollectionName, nameof(resourceCollectionName));
            Guard.NotNull(resourceDiscoveryRepository, nameof(resourceDiscoveryRepository));
            Guard.NotNullOrWhitespace(name, nameof(name));

            ResourceCollectionName = resourceCollectionName;

            _resourceDiscoveryRepository = resourceDiscoveryRepository;
        }

        public string ResourceCollectionName { get; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Scraping resource collection {ResourceCollection} - {Timestamp}", ResourceCollectionName, DateTimeOffset.UtcNow);

            try
            {
                var discoveredResources = await _resourceDiscoveryRepository.GetResourceCollectionAsync(ResourceCollectionName);
                Logger.LogInformation("Discovered {ResourceCount} resources for resource collection {ResourceCollection}.", discoveredResources?.Count, ResourceCollectionName);
            }
            catch (Exception exception)
            {
                Logger.LogCritical(exception, "Failed to scrape resource collection {ResourceCollection}: {Exception}", ResourceCollectionName, exception.Message);
            }
        }
    }
}