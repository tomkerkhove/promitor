using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    /// Used to deserialize a <see cref="SqlDatabaseResourceV1" /> resource.
    /// </summary>
    public class SqlDatabaseDeserializer : SqlServerDeserializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabaseDeserializer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SqlDatabaseDeserializer(ILogger logger) : base(logger)
        {
        }

        protected override SqlServerResourceV1 DeserializeResource(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var sqlServerResource = base.DeserializeResource(node, errorReporter);

            return new SqlDatabaseResourceV1(sqlServerResource)
            {
                DatabaseName = node.GetString("databaseName")
            };
        }
    }
}