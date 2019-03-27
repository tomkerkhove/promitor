using System;
using System.Collections.Generic;
using System.Text;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class CosmosDbMetricDefinition : MetricDefinition
    {
        public string TotalRequests { get; set; }
        public string DbName { get; set; }

        public override ResourceType ResourceType { get; } = ResourceType.CosmosDb;
    }
}
