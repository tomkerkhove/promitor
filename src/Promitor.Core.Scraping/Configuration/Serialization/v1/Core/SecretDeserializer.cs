﻿using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class SecretDeserializer : Deserializer<SecretV1>
    {
        private const string RawValueTag = "rawValue";
        private const string EnvironmentVariableTag = "environmentVariable";

        public SecretDeserializer(ILogger<SecretDeserializer> logger) : base(logger)
        {
        }

        public override SecretV1 Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var rawValue = node.GetString(RawValueTag);
            var environmentVariable = node.GetString(EnvironmentVariableTag);

            var secret = new SecretV1
            {
                RawValue = rawValue,
                EnvironmentVariable = environmentVariable
            };

            if (!string.IsNullOrEmpty(secret.RawValue) && !string.IsNullOrEmpty(secret.EnvironmentVariable))
            {
                Logger.LogWarning("Secret with environment variable '{EnvironmentVariable}' also has a rawValue provided.", secret.EnvironmentVariable);
            }

            return secret;
        }
    }
}
