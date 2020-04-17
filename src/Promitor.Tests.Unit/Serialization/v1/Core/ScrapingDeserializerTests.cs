using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Xunit;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class ScrapingDeserializerTests
    {
        private readonly ScrapingDeserializer _deserializer;

        public ScrapingDeserializerTests()
        {
            _deserializer = new ScrapingDeserializer(NullLogger<ScrapingDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_ScheduleSupplied_SetsSchedule()
        {
            const string yamlText =
@"scraping:
    schedule: '0 * * ? * *'";

            YamlAssert.PropertySet(
                _deserializer,
                yamlText,
                "scraping",
                "0 * * ? * *",
                s => s.Schedule);
        }

        [Fact]
        public void Deserialize_ScheduleNotSupplied_SetsScheduleNull()
        {
            const string yamlText =
@"scraping:
    otherProperty: otherValue";

            YamlAssert.PropertyNull(
                _deserializer,
                yamlText,
                "scraping",
                s => s.Schedule);
        }

        [Fact]
        public void Deserialize_ScheduleNotSupplied_ReportsError()
        {
            // Arrange
            var node = YamlUtils.CreateYamlNode("name: promitor");

            // Act / Assert
            YamlAssert.ReportsErrorForProperty(
                _deserializer,
                node,
                "schedule");
        }
    }
}
