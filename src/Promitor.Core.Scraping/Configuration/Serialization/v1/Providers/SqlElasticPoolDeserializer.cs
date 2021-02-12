using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    /// Used to deserialize a <see cref="SqlElasticPoolDeserializer" /> resource.
    /// </summary>
    public class SqlElasticPoolDeserializer : ResourceDeserializer<SqlElasticPoolResourceV1>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlElasticPoolDeserializer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SqlElasticPoolDeserializer(ILogger logger) : base(logger)
        {
            Map(resource => resource.ServerName)
                .IsRequired();
            Map(resource => resource.PoolName)
                .IsRequired();
        }
    }
}