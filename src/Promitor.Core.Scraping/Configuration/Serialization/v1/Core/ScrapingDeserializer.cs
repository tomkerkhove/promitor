using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class ScrapingDeserializer : Deserializer<ScrapingV1>
    {
        private const string ScheduleTag = "schedule";

        public ScrapingDeserializer(ILogger<ScrapingDeserializer> logger) : base(logger)
        {
        }

        public override ScrapingV1 Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var scraping = new ScrapingV1();

            scraping.Schedule = node.GetString(ScheduleTag);

            if (scraping.Schedule == null)
            {
                Logger.LogError("No default metric scraping schedule was configured!");
            }

            return scraping;
        }
    }
}
