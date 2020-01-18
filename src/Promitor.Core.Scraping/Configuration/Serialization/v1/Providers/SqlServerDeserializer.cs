using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    ///     Used to deserialize a <see cref="SqlServerDeserializer" /> resource.
    /// </summary>
    public class SqlServerDeserializer : ResourceDeserializer<SqlServerResourceV1>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlServerDeserializer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SqlServerDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override SqlServerResourceV1 DeserializeResource(YamlMappingNode node)
        {
            var serverName = node.GetString("serverName");

            return new SqlServerResourceV1
            {
                ServerName = serverName
            };
        }
    }
}