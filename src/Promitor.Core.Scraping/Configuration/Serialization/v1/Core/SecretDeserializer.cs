using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Serialization.v1.Model;
using YamlDotNet.RepresentationModel;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Core
{
    public class SecretDeserializer : Deserializer<SecretV1>
    {
        public SecretDeserializer(ILogger<SecretDeserializer> logger) : base(logger)
        {
            Map(secret => secret.RawValue);
            Map(secret => secret.EnvironmentVariable);
        }

        public override SecretV1 Deserialize(YamlMappingNode node, IErrorReporter errorReporter)
        {
            var secret = base.Deserialize(node, errorReporter);

            if (string.IsNullOrEmpty(secret.EnvironmentVariable) && string.IsNullOrEmpty(secret.RawValue))
            {
                errorReporter.ReportError(node, "Either 'environmentVariable' or 'rawValue' must be supplied for a secret.");
            }

            if (!string.IsNullOrEmpty(secret.RawValue) && !string.IsNullOrEmpty(secret.EnvironmentVariable))
            {
                errorReporter.ReportWarning(node, $"Secret with environment variable '{secret.EnvironmentVariable}' also has a rawValue provided.");
            }

            return secret;
        }
    }
}
