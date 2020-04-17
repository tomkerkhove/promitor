using AutoMapper;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Mapping;
using Xunit;

namespace Promitor.Tests.Unit.Serialization
{
    public class AutoMapperTests
    {
        [Fact]
        public void VerifyConfigurationsAreValid()
        {
            // Arrange
            var mapperConfiguration = new MapperConfiguration(c => c.AddProfile<V1MappingProfile>());

            // Act / Assert
            mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
