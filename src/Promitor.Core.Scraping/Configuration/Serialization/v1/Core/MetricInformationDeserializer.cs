using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class MetricInformationDeserializer : Deserializer<MetricInformationV1>
    {
        public MetricInformationDeserializer(ILogger<MetricInformationDeserializer> logger) : base(logger)
        {
            Map(definition => definition.Name);
            Map(definition => definition.Namespace);
        }
    }
}
