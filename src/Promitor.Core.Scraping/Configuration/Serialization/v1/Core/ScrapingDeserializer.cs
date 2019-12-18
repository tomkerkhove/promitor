using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class ScrapingDeserializer : Deserializer<ScrapingV1>
    {
        public ScrapingDeserializer(ILogger<ScrapingDeserializer> logger) : base(logger)
        {
            MapRequired(scraping => scraping.Schedule);
        }
    }
}
