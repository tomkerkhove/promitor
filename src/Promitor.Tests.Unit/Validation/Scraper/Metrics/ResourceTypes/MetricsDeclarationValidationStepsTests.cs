using AutoMapper;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;

namespace Promitor.Tests.Unit.Validation.Scraper.Metrics.ResourceTypes
{
    public class MetricsDeclarationValidationStepsTests : UnitTest
    {
        protected IMapper Mapper { get; }

        public MetricsDeclarationValidationStepsTests()
        {
            var config = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());
            Mapper = config.CreateMapper();
        }
    }
}