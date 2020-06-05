using GuardNet;
using Microsoft.Extensions.Logging;

namespace Promitor.Agents.Scraper.Scheduling
{
    public class MetricScrapingJob
    {
        public ILogger Logger { get; }

        public string Name { get; }

        public MetricScrapingJob(string name, ILogger logger)
        {
            Guard.NotNullOrWhitespace(name, nameof(name));
            Guard.NotNull(logger, nameof(logger));

            Name = name;
            Logger = logger;
        }
    }
}