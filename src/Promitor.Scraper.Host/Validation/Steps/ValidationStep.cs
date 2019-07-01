using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Promitor.Scraper.Host.Validation.Steps
{
    public class ValidationStep
    {
        protected IConfiguration Configuration { get; }
        protected ILogger Logger { get; }

        public ValidationStep(IConfiguration configuration, ILogger logger)
        {
            Configuration = configuration;
            Logger = logger;
        }
    }
}