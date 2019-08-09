using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    internal class SecretDeserializer : Deserializer<SecretV1>
    {
        internal SecretDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override SecretV1 Deserialize(YamlMappingNode node)
        {
            var secret = new SecretV1();

            if (node.Children.ContainsKey("rawValue"))
            {
                var rawValueNode = node.Children[new YamlScalarNode("rawValue")];
                secret.RawValue = rawValueNode.ToString();
            }

            if (node.Children.ContainsKey("environmentVariable"))
            {
                var rawEnvironmentVariable = node.Children[new YamlScalarNode("environmentVariable")];
                secret.EnvironmentVariable = rawEnvironmentVariable.ToString();
            }

            return secret;
        }
    }
}
