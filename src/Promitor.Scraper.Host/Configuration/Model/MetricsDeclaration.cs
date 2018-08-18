using System.Collections.Generic;
using Promitor.Scraper.Host.Configuration.Model.Metrics.ResouceTypes;

namespace Promitor.Scraper.Host.Configuration.Model
{
    public class MetricsDeclaration
    {
        public AzureMetadata AzureMetadata { get; set; }
        public List<ServiceBusQueueMetricDefinition> Metrics { get; set; } = new List<ServiceBusQueueMetricDefinition>();
    }
}