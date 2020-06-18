using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    /// Used to deserialize a <see cref="SqlDatabaseResourceV1" /> resource.
    /// </summary>
    public class SqlDatabaseDeserializer : ResourceDeserializer<SqlDatabaseResourceV1>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabaseDeserializer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SqlDatabaseDeserializer(ILogger logger) : base(logger)
        {
            Map(resource => resource.ServerName)
                .IsRequired();
            Map(resource => resource.DatabaseName)
                .IsRequired();
        }
    }
}