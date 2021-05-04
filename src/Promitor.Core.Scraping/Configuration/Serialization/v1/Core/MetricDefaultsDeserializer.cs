using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricDefaultsDeserializer : Deserializer<MetricDefaultsV1>
    {
        public MetricDefaultsDeserializer(
            IDeserializer<AggregationV1> aggregationDeserializer,
            IDeserializer<ScrapingV1> scrapingDeserializer,
            ILogger<MetricDefaultsDeserializer> logger) : base(logger)
        {
            Map(defaults => defaults.Aggregation)
                .MapUsingDeserializer(aggregationDeserializer);
            Map(defaults => defaults.Limit)
                .MapUsing(DetermineLimit);
            Map(defaults => defaults.Scraping)
                .IsRequired()
                .MapUsingDeserializer(scrapingDeserializer);
            Map(defaults => defaults.Labels);
        }

        private object DetermineLimit(string rawLimit, KeyValuePair<YamlNode, YamlNode> nodePair, IErrorReporter errorReporter)
        {
            if (int.TryParse(rawLimit, out int limit))
            {
                return limit;
            }

            return null;
        }
    }
}
