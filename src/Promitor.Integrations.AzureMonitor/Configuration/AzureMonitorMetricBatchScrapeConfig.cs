using System.Diagnostics.CodeAnalysis;

namespace Promitor.Integrations.AzureMonitor.Configuration
{
    public class AzureMonitorMetricBatchScrapeConfig
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Explicit initialization to false for better readability")]
        public bool Enabled { get; set; } = false; 
        public int MaxBatchSize { get; set; }
        public string AzureRegion { get; set; } // Batch scrape endpoints are deployed by region 
    }
}
