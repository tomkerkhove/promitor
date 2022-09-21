using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class LogAnalyticsConfigurationDeserializerTests : UnitTest
    {
        private readonly LogAnalyticsConfigurationDeserializer _deserializer;

        public LogAnalyticsConfigurationDeserializerTests()
        {
            _deserializer = new LogAnalyticsConfigurationDeserializer(NullLogger<LogAnalyticsConfigurationDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_QuerySupplied_SetsQuery()
        {
            YamlAssert.PropertySet(
                _deserializer,
                "query: AzureActivity | take 100",
                "AzureActivity | take 100",
                a => a.Query);
        }

        [Fact]
        public void Deserialize_QueryNotSupplied_ReportsError()
        {
            var node = YamlUtils.CreateYamlNode("field: promitor");

            YamlAssert.ReportsErrorForProperty(_deserializer, node, "query");
        }
    }
}
