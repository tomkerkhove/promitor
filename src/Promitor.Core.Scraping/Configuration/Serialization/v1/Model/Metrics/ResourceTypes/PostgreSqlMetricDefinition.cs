namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics.ResourceTypes
{
    public class PostgreSqlMetricDefinition : MetricDefinition
    {
        public string ServerName { get; set; }
        public override ResourceType ResourceType { get; } = this.ResourceType.PostgreSql;
    }
}
