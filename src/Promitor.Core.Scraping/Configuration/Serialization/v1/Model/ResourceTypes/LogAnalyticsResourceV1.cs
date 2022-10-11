namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes
{
    public class LogAnalyticsResourceV1 : AzureResourceDefinitionV1
    {
        /// <summary>
        /// The workspaceId of the Azure Log Analytics to query metrics for.
        /// </summary>
        public string WorkspaceId { get; set; }

        /// <summary>
        /// The name of the Azure Log Analytics to query metrics for.
        /// </summary>
        public string WorkspaceName { get; set; }
    }
}