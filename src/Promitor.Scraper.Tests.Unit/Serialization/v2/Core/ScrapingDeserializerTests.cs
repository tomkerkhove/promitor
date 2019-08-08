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
            const string yamlText =
@"scraping:
    schedule: '0 * * ? * *'";

            DeserializerTestHelpers.AssertPropertySet(
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

            DeserializerTestHelpers.AssertPropertyNull(
                _deserializer,
                yamlText,
                "scraping",
                s => s.Schedule);
        }
    }
}
