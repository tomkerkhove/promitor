using AutoMapper;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Mapping;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization
{
    public class AutoMapperTests
    {
        [Fact]
        public void VerifyConfigurationsAreValid()
        {
            // Arrange
            var mapperConfiguration = new MapperConfiguration(c => c.AddProfile<V2MappingProfile>());

            // Act / Assert
            mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
