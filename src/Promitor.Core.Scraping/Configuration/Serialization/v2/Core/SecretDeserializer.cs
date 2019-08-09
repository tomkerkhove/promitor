using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v2.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Core
{
    public class SecretDeserializer : Deserializer<SecretV2>
    {
        private const string RawValueTag = "rawValue";
        private const string EnvironmentVariableTag = "environmentVariable";

        public SecretDeserializer(ILogger logger) : base(logger)
        {
        }

        public override SecretV2 Deserialize(YamlMappingNode node)
        {
            var secret = new SecretV2
            {
                RawValue = GetString(node, RawValueTag),
                EnvironmentVariable = GetString(node, EnvironmentVariableTag)
            };

            if (!string.IsNullOrEmpty(secret.RawValue) && !string.IsNullOrEmpty(secret.EnvironmentVariable))
            {
                Logger.LogWarning("Secret with environment variable '{EnvironmentVariable}' also has a rawValue provided.", secret.EnvironmentVariable);
            }

            return secret;
        }
    }
}
