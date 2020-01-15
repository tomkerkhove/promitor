namespace Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes
{
    public class PostgreSqlResourceDefinition : AzureResourceDefinition
    {
        public PostgreSqlResourceDefinition(string resourceGroupName, string serverName)
            : base(ResourceType.PostgreSql, resourceGroupName)
        {
            ServerName = serverName;
        }

        public string ServerName { get; }

        /// <inheritdoc />
        public override string GetResourceName() => ServerName;
    }
}
