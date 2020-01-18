using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class CosmosDbDeserializer : ResourceDeserializer<CosmosDbResourceV1>
    {
        private const string DatabaseNameTag = "dbName";

        public CosmosDbDeserializer(ILogger<CosmosDbDeserializer> logger) : base(logger)
        {
        }

        protected override CosmosDbResourceV1 DeserializeResource(YamlMappingNode node)
        {
            var databaseName = node.GetString(DatabaseNameTag);

            return new CosmosDbResourceV1
            {
                DbName = databaseName
            };
        }
    }
}
