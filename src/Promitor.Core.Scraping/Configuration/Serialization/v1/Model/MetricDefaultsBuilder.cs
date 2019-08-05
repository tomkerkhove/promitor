using Promitor.Core.Scraping.Configuration.Model;
using YamlDotNet.Serialization;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class MetricDefaultsBuilder
    {
        [YamlMember(Alias = "aggregation")]
        public AggregationBuilder AggregationBuilder { get; set; } = new AggregationBuilder();

        [YamlMember(Alias = "scraping")]
        public ScrapingBuilder ScrapingBuilder { get; set; } = new ScrapingBuilder();

        public MetricDefaults Build()
        {
            return new MetricDefaults
            {
                Aggregation = AggregationBuilder.Build(),
                Scraping = ScrapingBuilder.Build()
            };
        }
    }
}