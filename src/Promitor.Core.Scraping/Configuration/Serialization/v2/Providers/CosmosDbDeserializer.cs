using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    public class CosmosDbDeserializer : ResourceDeserializer
    {
        private const string DatabaseNameTag = "dbName";

        public CosmosDbDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV2 DeserializeResource(YamlMappingNode node)
        {
            return new CosmosDbResourceV2
            {
                DbName = GetString(node, DatabaseNameTag)
            };
        }
    }
}
