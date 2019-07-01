using Microsoft.Extensions.Configuration;

namespace Promitor.Scraper.Tests.Unit.Validation.Metrics
{
    public class MetricsDeclarationValidationStepTests
    {
        public IConfiguration Configuration { get; }

        public  MetricsDeclarationValidationStepTests()
        {
            Configuration = new ConfigurationBuilder()
                .Build();
        }
    }
}
