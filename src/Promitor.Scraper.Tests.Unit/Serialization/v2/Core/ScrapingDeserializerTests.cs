using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Core;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Scraper.Tests.Unit.Serialization.v2.Core
{
    [Category("Unit")]
    public class ScrapingDeserializerTests
    {
        private readonly ScrapingDeserializer _deserializer;

        public ScrapingDeserializerTests()
        {
            _deserializer = new ScrapingDeserializer(new Mock<ILogger>().Object);
        }

        [Fact]
        public void Deserialize_ScheduleSupplied_SetsSchedule()
        {
            // Arrange
            const string yamlText =
@"scraping:
    schedule: '0 * * ? * *'";
            var node = (YamlMappingNode)YamlUtils.CreateYamlNode(yamlText).Children["scraping"];

            // Act
            var scraping = _deserializer.Deserialize(node);

            // Assert
            Assert.Equal("0 * * ? * *", scraping.Schedule);
        }

        [Fact]
        public void Deserialize_ScheduleNotSupplied_SetsScheduleNull()
        {
            // Arrange
            const string yamlText =
@"scraping:
    otherProperty: otherValue";
            var node = (YamlMappingNode)YamlUtils.CreateYamlNode(yamlText).Children["scraping"];

            // Act
            var scraping = _deserializer.Deserialize(node);

            // Assert
            Assert.Null(scraping.Schedule);
        }
    }
}
