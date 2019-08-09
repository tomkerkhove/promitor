using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class PostgreSqlMetricDefinitionV1 : MetricDefinitionV1
    {
        public string ServerName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.PostgreSql;

        public override MetricDefinition Build()
        {
            return new PostgreSqlMetricDefinition(
                AzureMetricConfiguration.Build(),
                Description,
                Name,
                ResourceGroupName,
                ServerName,
                Labels,
                Scraping.Build());
        }
    }
}
