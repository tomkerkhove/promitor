namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class PostgreSqlMetricDefinition : MetricDefinition
    {
        public string ServerName { get; set; }
        public override ResourceType ResourceType { get; } = ResourceType.PostgreSql;
    }
}
