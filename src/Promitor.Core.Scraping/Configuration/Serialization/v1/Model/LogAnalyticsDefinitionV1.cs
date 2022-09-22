using System.Collections.Generic;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class LogAnalyticsDefinitionV1
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Dictionary<string, string> Labels { get; set; }

        public ScrapingV1 Scraping { get; set; }

        public string Query { get; set; }

        public string WorkspaceId { get; set; }
    }
}