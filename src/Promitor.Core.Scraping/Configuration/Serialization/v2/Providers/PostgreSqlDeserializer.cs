using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Providers
{
    public class PostgreSqlDeserializer : ResourceDeserializer
    {
        private const string ServerNameTag = "serverName";

        public PostgreSqlDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV2 DeserializeResource(YamlMappingNode node)
        {
            return new PostgreSqlResourceV2
            {
                ServerName = GetString(node, ServerNameTag)
            };
        }
    }
}
