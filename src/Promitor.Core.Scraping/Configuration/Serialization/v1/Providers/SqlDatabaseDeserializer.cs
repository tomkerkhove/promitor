using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    /// Used to deserialize a <see cref="SqlDatabaseResourceV1" /> resource.
    /// </summary>
    public class SqlDatabaseDeserializer : ResourceDeserializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabaseDeserializer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SqlDatabaseDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override AzureResourceDefinitionV1 DeserializeResource(YamlMappingNode node)
        {
            var serverName = node.GetString("serverName");
            var databaseName = node.GetString("databaseName");

            return new SqlDatabaseResourceV1
            {
                ServerName = serverName,
                DatabaseName = databaseName
            };
        }
    }
}