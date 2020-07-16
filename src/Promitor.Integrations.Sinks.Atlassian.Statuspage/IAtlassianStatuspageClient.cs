using System.Threading.Tasks;

namespace Promitor.Integrations.Sinks.Atlassian.Statuspage
{
    public interface IAtlassianStatuspageClient
    {
        Task ReportMetricAsync(string id, double value);
    }
}