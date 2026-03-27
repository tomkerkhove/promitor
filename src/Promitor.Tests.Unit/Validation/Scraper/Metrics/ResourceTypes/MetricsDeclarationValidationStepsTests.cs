using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;

namespace Promitor.Tests.Unit.Validation.Scraper.Metrics.ResourceTypes
{
    public class MetricsDeclarationValidationStepsTests : UnitTest
    {
        protected V1ConfigurationMapper Mapper { get; }

        public MetricsDeclarationValidationStepsTests()
        {
            Mapper = new V1ConfigurationMapper();
        }
    }
}