using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.ResourceTypes;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Providers
{
    public class AutomationAccountDeserializer : ResourceDeserializer<AutomationAccountResourceV1>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutomationAccountDeserializer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public AutomationAccountDeserializer(ILogger logger) : base(logger)
        {
            Map(resource => resource.AccountName)
                .IsRequired();
        }
    }
}
