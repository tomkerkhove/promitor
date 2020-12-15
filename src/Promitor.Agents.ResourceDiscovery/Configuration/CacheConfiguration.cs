namespace Promitor.Agents.ResourceDiscovery.Configuration
{
    public class CacheConfiguration
    {
        public bool Enabled { get; set; } = true;
        public int DurationInMinutes { get; set; } = 5;
    }
}
