using DefaultsCore = Promitor.Agents.Core.Configuration.Defaults;

namespace Promitor.Agents.Scraper.Configuration
{
    public class ConcurrencyConfiguration
    {
        public int MutexTimeoutSeconds { get; set; } = DefaultsCore.Concurrency.MutexTimeoutSeconds;
    }
}
