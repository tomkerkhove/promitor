using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class ScrapingDeserializer : Deserializer<ScrapingV2>
    {
        private const string ScheduleTag = "schedule";

        public ScrapingDeserializer(ILogger logger) : base(logger)
        {
        }

        public override ScrapingV2 Deserialize(YamlMappingNode node)
        {
            var scraping = new ScrapingV2();

            scraping.Schedule = GetString(node, ScheduleTag);

            if (scraping.Schedule == null)
            {
                // TODO: this will log an error if this deserializer is reused to deserialize the scraping settings for a metric instead of the global scraping settings. Need to fix that!
                Logger.LogError("No default metric scraping schedule was configured!");
            }

            return scraping;
        }
    }
}
