using Promitor.Scraper.Configuration.Model;

namespace Promitor.Scraper.Configuration.Providers.Interfaces
{
    public interface IScrapeConfigurationProvider
    {
        /// <summary>
        ///     Gets scraping configuration
        /// </summary>
        ScrapeConfiguration GetConfiguration();
    }
}