using System.Collections.Generic;
using Promitor.Scraper.Model.Configuration.Metrics.ResouceTypes;

namespace Promitor.Scraper.Model.Configuration
{
    public class MetricsDeclaration
    {
        public AzureMetadata AzureMetadata { get; set; }
        public List<ServiceBusQueueMetricDefinition> Metrics { get; set; } = new List<ServiceBusQueueMetricDefinition>();
    }
}