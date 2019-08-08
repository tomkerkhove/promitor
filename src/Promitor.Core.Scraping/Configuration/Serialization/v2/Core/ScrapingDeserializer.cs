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

            if (node.Children.TryGetValue(ScheduleTag, out var scrapingNode))
            {
                scraping.Schedule = scrapingNode.ToString();
            }
            else
            {
                Logger.LogError("No default metric scraping schedule was configured!");
            }

            return scraping;
        }
    }
}
