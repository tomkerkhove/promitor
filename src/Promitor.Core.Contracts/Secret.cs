using System;

namespace Promitor.Core.Contracts
{
    public class Secret
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
    }
}
