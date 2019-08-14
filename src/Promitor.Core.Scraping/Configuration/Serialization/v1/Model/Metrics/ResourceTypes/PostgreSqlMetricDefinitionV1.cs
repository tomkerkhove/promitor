using Promitor.Core.Scraping.Configuration.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class PostgreSqlMetricDefinitionV1 : MetricDefinitionV1
    {
        public string ServerName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.PostgreSql;
    }
}
