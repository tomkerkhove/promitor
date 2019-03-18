using Microsoft.Extensions.Logging;

namespace Promitor.Scraper.Host.Validation.Steps
{
    public class ValidationStep
    {
        protected ILogger Logger { get; }

        public ValidationStep(ILogger logger)
        {
            Logger = logger;
        }
    }
}