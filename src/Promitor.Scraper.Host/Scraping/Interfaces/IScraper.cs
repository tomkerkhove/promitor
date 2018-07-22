using System.Threading.Tasks;
using Promitor.Scraper.Host.Configuration.Model.Metrics;

namespace Promitor.Scraper.Host.Scraping.Interfaces
{
    internal interface IScraper<in TMetricDefinition> where TMetricDefinition : MetricDefinition
    {
        Task ScrapeAsync(TMetricDefinition metricDefinition);
    }
}