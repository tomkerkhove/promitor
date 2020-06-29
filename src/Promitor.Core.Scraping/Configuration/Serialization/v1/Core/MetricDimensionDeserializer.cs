using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricDimensionDeserializer : Deserializer<MetricDimensionV1>
    {
        public MetricDimensionDeserializer(ILogger<MetricDimensionDeserializer> logger)
            : base(logger)
        {
            Map(dimension => dimension.Name);
        }
    }
}
