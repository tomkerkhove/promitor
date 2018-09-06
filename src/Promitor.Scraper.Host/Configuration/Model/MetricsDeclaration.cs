using System.Collections.Generic;
using Promitor.Scraper.Host.Configuration.Model.Metrics;

namespace Promitor.Scraper.Host.Configuration.Model
{
    public class MetricsDeclaration
    {
        public AzureMetadata AzureMetadata { get; set; }
        public List<MetricDefinition> Metrics { get; set; } = new List<MetricDefinition>();
    }
}