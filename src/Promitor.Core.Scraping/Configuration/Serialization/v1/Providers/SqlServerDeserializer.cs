using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

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
            Map(resource => resource.ServerName)
                .IsRequired();
        }
    }
}