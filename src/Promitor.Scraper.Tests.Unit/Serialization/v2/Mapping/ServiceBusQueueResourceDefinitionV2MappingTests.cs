using System.ComponentModel;
using AutoMapper;
using Promitor.Core.Scraping.Configuration.Model.Metrics.ResourceTypes;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Mapping;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model.ResourceTypes;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Mapping
{
    [Category("Unit")]
    public class ServiceBusQueueResourceDefinitionV2MappingTests
    {
        [Fact]
        public void Map_CanMapServiceBusQueueResource()
        {
            // Arrange
            var mapperConfiguration = new MapperConfiguration(c => c.AddProfile<V2MappingProfile>());
            var mapper = mapperConfiguration.CreateMapper();

            var v2Resource = new ServiceBusQueueResourceV2 {Namespace = "test-namespace"};

            // Act
            var resource = mapper.Map<ServiceBusQueueResourceDefinition>(v2Resource);

            // Assert
            Assert.Equal(v2Resource.Namespace, resource.Namespace);
        }
    }
}
