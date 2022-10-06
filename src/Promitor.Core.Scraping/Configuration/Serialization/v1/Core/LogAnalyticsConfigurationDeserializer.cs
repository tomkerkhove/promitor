using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class LogAnalyticsConfigurationDeserializer : Deserializer<LogAnalyticsConfigurationV1>
    {
        public LogAnalyticsConfigurationDeserializer(IDeserializer<AggregationV1> aggregationDeserializer, ILogger<LogAnalyticsConfigurationDeserializer> logger) : base(logger)
        {
            Map(logAnalyticsConfiguration => logAnalyticsConfiguration.Query)
                .IsRequired();
            Map(logAnalyticsConfiguration => logAnalyticsConfiguration.Aggregation)
                .MapUsingDeserializer(aggregationDeserializer);
        }
    }
}