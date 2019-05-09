using System;

namespace Promitor.Core.Infrastructure
{
    public class FeatureFlag
    {
        /// <summary>
        ///     Determine if a feature flag is active or not
        /// </summary>
        /// <param name="toggleName">Name of the feature flag</param>
        /// <param name="defaultFlagState">Default state of the feature flag if it's not configured</param>
        public static bool IsActive(string toggleName, bool defaultFlagState = true)
        {
            var environmentVariableName = $"PROMITOR_FEATURE_{toggleName}";
            var rawFlag = Environment.GetEnvironmentVariable(environmentVariableName);

            if (bool.TryParse(rawFlag, out var isActive))
            {
                return isActive;
            }

            return defaultFlagState;
        }

        public static class Names
        {
            public static string MetricsTimestamp { get; } = "METRICSTIMESTAMP";
        }
    }
}