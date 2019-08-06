namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class PostgreSqlMetricDefinition : AzureResourceDefinition
    {
        public PostgreSqlMetricDefinition() : base(ResourceType.PostgreSql)
        {
        }

        public PostgreSqlMetricDefinition(string resourceGroupName, string serverName)
            : base(ResourceType.PostgreSql, resourceGroupName)
        {
            ServerName = serverName;
        }

        public string ServerName { get; set; }
    }
}
