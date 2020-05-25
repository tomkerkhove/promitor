namespace Promitor.Agents.Scraper.Configuration
{
    public class ResourceDiscoveryConfiguration
    {
        public string Host { get; set; }
        public int? Port { get; set; } = 80;

        public bool IsConfigured => string.IsNullOrWhiteSpace(Host) == false;
    }
}
