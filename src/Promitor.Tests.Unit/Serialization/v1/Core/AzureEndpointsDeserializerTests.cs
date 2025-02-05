using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Core;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace Promitor.Tests.Unit.Serialization.v1.Core
{
    [Category("Unit")]
    public class AzureEndpointsDeserializerTests : UnitTest
    {
        private readonly AzureEndpointsDeserializer _deserializer;
        public AzureEndpointsDeserializerTests() 
        {
            _deserializer = new AzureEndpointsDeserializer(NullLogger<AzureEndpointsDeserializer>.Instance);
        }

        [Fact]
        public void Deserialize_AzureEndpointsSuppliedWithInvalidUrl_Fails()
        {
            var authEndpoint = "auth.end.point";
            var yamlNode = YamlUtils.CreateYamlNode(
                $@"endpoints:
    authenticationEndpoint: '{authEndpoint}'");

            var endpointsNode = (YamlMappingNode)yamlNode.Children["endpoints"];
            var errorNode = endpointsNode.Children["authenticationEndpoint"];

            YamlAssert.ReportsError(
                _deserializer,
                endpointsNode,
                errorNode,
                $"'{authEndpoint}' is not a valid URL.");
        }
    }
}
