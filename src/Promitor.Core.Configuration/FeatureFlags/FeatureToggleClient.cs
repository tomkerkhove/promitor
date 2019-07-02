using System;
using Microsoft.Extensions.Options;
using Promitor.Core.Configuration.Model.FeatureFlags;

namespace Promitor.Core.Configuration.FeatureFlags
{
    public class FeatureToggleClient
    {
        private readonly IOptionsMonitor<FeatureFlagsConfiguration> _featureFlagConfiguration;

        public FeatureToggleClient(IOptionsMonitor<FeatureFlagsConfiguration> featureFlagConfiguration)
        {
            _featureFlagConfiguration = featureFlagConfiguration;
        }

        /// <summary>
        ///     Determine if a feature flag is active or not
        /// </summary>
        /// <param name="toggleName">Name of the feature flag</param>
        /// <param name="defaultFlagState">Default state of the feature flag if it's not configured</param>
        public bool IsActive(ToggleNames toggleName, bool defaultFlagState = true)
        {
            var featureFlagConfiguration = _featureFlagConfiguration.CurrentValue;
            if (featureFlagConfiguration == null)
            {
                return defaultFlagState;
            }

            switch (toggleName)
            {
                case ToggleNames.DisableMetricTimestamps:
                    return featureFlagConfiguration.DisableMetricTimestamps;
                default:
                    throw new Exception($"Unable to determine feature flag state for '{toggleName}'");
            }
        }
    }
}