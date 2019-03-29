using System.Threading.Tasks;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.Interfaces
{
    public interface IScraper<in TMetricDefinition> where TMetricDefinition : MetricDefinition
    {
        Task<int> ScrapeAsync(TMetricDefinition metricDefinition);
    }
}