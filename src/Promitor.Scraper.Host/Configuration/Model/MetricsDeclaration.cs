using System.Collections.Generic;
using Promitor.Scraper.Host.Model.Configuration;
using Promitor.Scraper.Model.Configuration.Metrics.ResouceTypes;

namespace Promitor.Scraper.Host.Configuration.Model
{
    public class MetricsDeclaration
    {
        public AzureMetadata AzureMetadata { get; set; }
        public List<ServiceBusQueueMetricDefinition> Metrics { get; set; } = new List<ServiceBusQueueMetricDefinition>();
    }
}