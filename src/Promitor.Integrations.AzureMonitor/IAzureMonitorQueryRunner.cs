using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent;
using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Integrations.AzureMonitor
{
    public interface IAzureMonitorQueryRunner
    {
        Task<IEnumerable<MetricValue>> RunQuery(string metricName, IMetricDefinition metricDefinition, QueryOptions options);
    }
}