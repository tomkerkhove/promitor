using Microsoft.Extensions.Logging;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.Core
{
    internal class ScrapingDeserializer : Deserializer<Model.Scraping>
    {
        internal ScrapingDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override Model.Scraping Deserialize(YamlMappingNode node)
        {
            var scraping = new Model.Scraping();

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
