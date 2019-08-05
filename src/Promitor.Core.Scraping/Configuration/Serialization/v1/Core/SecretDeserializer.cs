﻿using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    internal class SecretDeserializer : Deserializer<SecretBuilder>
    {
        internal SecretDeserializer(ILogger logger) : base(logger)
        {
        }

        internal override SecretBuilder Deserialize(YamlMappingNode node)
        {
            var secret = new SecretBuilder();

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
