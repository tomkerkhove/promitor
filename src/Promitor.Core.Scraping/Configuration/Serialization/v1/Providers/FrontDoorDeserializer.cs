using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    /// <summary>
    /// Used to deserialize a <see cref="FrontDoorDeserializer" /> resource.
    /// </summary>
    public class FrontDoorDeserializer : ResourceDeserializer<FrontDoorResourceV1>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrontDoorDeserializer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FrontDoorDeserializer(ILogger logger) : base(logger)
        {
            Map(resource => resource.Name)
                .IsRequired();
        }
    }
}
