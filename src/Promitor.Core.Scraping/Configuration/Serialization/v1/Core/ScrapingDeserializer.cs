using Microsoft.Extensions.Logging;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    internal class ScrapingDeserializer : Deserializer<Model.ScrapingBuilder>
    {
        internal ScrapingDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override Model.ScrapingBuilder Deserialize(YamlMappingNode node)
        {
            var scraping = new Model.ScrapingBuilder();

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
