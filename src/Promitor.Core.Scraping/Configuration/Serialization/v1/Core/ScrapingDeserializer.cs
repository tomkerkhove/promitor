using Microsoft.Extensions.Logging;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    internal class ScrapingDeserializer : Deserializer<Model.ScrapingV1>
    {
        internal ScrapingDeserializer(ILogger logger) : base(logger)
        {
        }

        public override Model.ScrapingV1 Deserialize(YamlMappingNode node)
        {
            var scraping = new Model.ScrapingV1();

            if (node.Children.ContainsKey("schedule"))
            {
                var rawScheduleNode = node.Children[new YamlScalarNode("schedule")];
                scraping.Schedule = rawScheduleNode.ToString();
            }
            else
            {
                Logger.LogError("No default metric scraping schedule was configured!");
            }

            return scraping;
        }
    }
}
