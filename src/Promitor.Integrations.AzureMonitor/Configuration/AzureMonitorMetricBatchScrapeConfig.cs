using System.Diagnostics.CodeAnalysis;

namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorMetricBatchScrapeConfig
    {
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Explicitly init the false value for better readability")]
        public bool Enabled { get; set; } = false; 
        public int MaxBatchSize { get; set; }
        public string AzureRegion { get; set; } // Batch scrape endpoints are deployed by region 
    }
}