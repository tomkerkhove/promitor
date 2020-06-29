using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    /// Used to deserialize a <see cref="ApiManagementDeserializer" /> resource.
    /// </summary>
    public class ApiManagementDeserializer : ResourceDeserializer<ApiManagementResourceV1>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiManagementDeserializer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ApiManagementDeserializer(ILogger logger) : base(logger)
        {
            Map(resource => resource.InstanceName)
                .IsRequired();
            Map(resource => resource.LocationName);
        }
    }
}
