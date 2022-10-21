
namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class LogAnalyticsConfigurationV1
    {
        public string Query { get; set; }

        public AggregationV1 Aggregation { get; set; }
    }
}