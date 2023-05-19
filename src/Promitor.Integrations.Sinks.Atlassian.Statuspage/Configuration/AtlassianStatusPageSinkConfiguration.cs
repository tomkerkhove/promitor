using System.Collections.Generic;

namespace Promitor.Integrations.Sinks.Atlassian.Statuspage.Configuration
{
    public class AtlassianStatusPageSinkConfiguration
    {
        public string PageId { get; set; }
        public List<SystemMetricMapping> SystemMetricMapping { get; set; } = new();
    }
}