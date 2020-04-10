using Microsoft.Extensions.Logging;

namespace Promitor.Runtime.Agents.Scraper.Validation.Steps
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