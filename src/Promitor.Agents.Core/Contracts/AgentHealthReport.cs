using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Promitor.Agents.Core.Contracts
{
    public class AgentHealthReport
    {
        public HealthStatus Status { get; set; }
    }
}
