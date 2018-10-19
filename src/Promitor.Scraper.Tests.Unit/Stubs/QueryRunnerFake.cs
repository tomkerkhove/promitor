using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor.Fluent;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Promitor.Integrations.AzureMonitor;

namespace Promitor.Scraper.Tests.Unit.Stubs
{
    public class QueryRunnerFake : IAzureMonitorQueryRunner
    {
        private readonly IEnumerable<MetricValue> _values;

        public QueryRunnerFake(IEnumerable<MetricValue> values)
        {
            this._values = values;
        }

        public Task<IEnumerable<MetricValue>> RunQuery(string metricName, IMetricDefinition metricDefinition, QueryOptions options)
        {
            return Task.FromResult(_values);
        }
    }
}