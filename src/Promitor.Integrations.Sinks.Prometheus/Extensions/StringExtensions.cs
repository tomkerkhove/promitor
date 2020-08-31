// ReSharper disable once CheckNamespace

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Ensures label key name is valid according to Prometheus requirements
        /// </summary>
        /// <param name="labelKey">Current label key value</param>
        public static string SanitizeForPrometheusLabelKey(this string labelKey)
        {
            if (string.IsNullOrWhiteSpace(labelKey))
            {
                return labelKey;
            }

            return labelKey.Replace("/", "_").ToLower();
        }
    }
}