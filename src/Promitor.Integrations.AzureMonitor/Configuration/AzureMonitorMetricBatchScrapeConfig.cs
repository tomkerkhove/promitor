namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorMetricBatchScrapeConfig
    {
        public bool Enabled { get; set; }
        public int MaxBatchSize { get; set; }
    }
}