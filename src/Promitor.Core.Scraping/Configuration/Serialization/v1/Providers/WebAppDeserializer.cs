using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class WebAppDeserializer : ResourceDeserializer<WebAppResourceV1>
    {
        private const string WebAppNameTag = "webAppName";
        private const string SlotNameTag = "slotName";

        public WebAppDeserializer(ILogger<WebAppDeserializer> logger) : base(logger)
        {
        }

        protected override WebAppResourceV1 DeserializeResource(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var webAppName = node.GetString(WebAppNameTag);
            var slotName = node.GetString(SlotNameTag);

            return new WebAppResourceV1
            {
                WebAppName = webAppName,
                SlotName = slotName,
            };
        }
    }
}
