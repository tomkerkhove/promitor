using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class FunctionAppDeserializer : ResourceDeserializer
    {
        private const string FunctionAppNameTag = "functionAppName";
        private const string SlotNameTag = "slotName";

        public FunctionAppDeserializer(ILogger<FunctionAppDeserializer> logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            var functionAppName = node.GetString(FunctionAppNameTag);
            var slotName = node.GetString(SlotNameTag);

            return new FunctionAppResourceV1
            {
                FunctionAppName = functionAppName,
                SlotName = slotName
            };
        }
    }
}
