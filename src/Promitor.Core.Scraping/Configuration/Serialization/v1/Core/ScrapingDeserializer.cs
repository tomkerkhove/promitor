using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class ScrapingDeserializer : Deserializer<ScrapingV1>
    {
        public ScrapingDeserializer(ILogger<ScrapingDeserializer> logger) : base(logger)
        {
            Map(scraping => scraping.Schedule)
                .IsRequired()
                .ValidateCronExpression();
        }
    }
}
