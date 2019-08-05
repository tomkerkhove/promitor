using System;
using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics
{
    public class SecretBuilder
    {
        public string RawValue { get; set; }
        public string EnvironmentVariable { get; set; }

        /// <summary>
        /// Provides the secret value based on the configured approach
        /// </summary>
        public string GetSecretValue()
        {
            if (string.IsNullOrWhiteSpace(EnvironmentVariable) == false)
            {
                var secretValue = Environment.GetEnvironmentVariable(EnvironmentVariable);
                return secretValue;
            }

            if (string.IsNullOrWhiteSpace(RawValue) == false)
            {
                return RawValue;
            }

            return string.Empty;
        }

        public Secret Build()
        {
            return new Secret
            {
                EnvironmentVariable = EnvironmentVariable,
                RawValue = RawValue
            };
        }
    }
}
