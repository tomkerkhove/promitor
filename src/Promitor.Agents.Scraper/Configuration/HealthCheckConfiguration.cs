namespace Promitor.Agents.Scraper.Configuration
{
    public class HealthCheckConfiguration
    {
        /// <summary>
        /// Indicates whether the scraper freshness health check is enabled.
        /// This health check monitors whether scrapes have been completed recently.
        /// Defaults to true.
        /// </summary>
        public bool EnableScraperFreshnessHealthCheck { get; set; } = true;
    }
}

