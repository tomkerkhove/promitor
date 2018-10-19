using Microsoft.Azure.Management.Monitor.Fluent.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Promitor.Integrations.AzureMonitor
{
    public class QueryOptions
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AggregationType AggregationType { get; set; }
        public TimeSpan MetricGranularity { get; set; }
        public string MetricFilter { get; set; }
    }
}
