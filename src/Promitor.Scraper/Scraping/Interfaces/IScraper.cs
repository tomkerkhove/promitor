using System.Threading.Tasks;
using Promitor.Scraper.Model.Configuration.Metrics;

namespace Promitor.Scraper.Scraping.Interfaces
{
    internal interface IScraper<in TMetricDefinition> where TMetricDefinition : MetricDefinition
    {
        Task ScrapeAsync(TMetricDefinition metricDefinition);
    }
}