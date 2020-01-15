using System.Threading.Tasks;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.Interfaces
{
    public interface IScraper<TResourceDefinition> where TResourceDefinition : class, IAzureResourceDefinition
    {
        Task ScrapeAsync(ScrapeDefinition<TResourceDefinition> scrapeDefinition);
    }
}