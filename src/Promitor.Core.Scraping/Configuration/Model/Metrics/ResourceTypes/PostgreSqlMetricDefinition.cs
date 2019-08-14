using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class PostgreSqlMetricDefinition : MetricDefinition
    {
        public PostgreSqlMetricDefinition()
        {
        }

        public PostgreSqlMetricDefinition(AzureMetricConfiguration azureMetricConfiguration, string description, string name, string resourceGroupName, string serverName, Dictionary<string, string> labels, Scraping scraping)
            : base(name, description, resourceGroupName, labels, scraping, azureMetricConfiguration)
        {
            ServerName = serverName;
        }

        public string ServerName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.PostgreSql;
    }
}
