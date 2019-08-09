namespace Promitor.Core.Scraping.Configuration.Serialization.v2.Model
{
    public class SecretV2
    {
        /// <summary>
        /// The value of the secret. If you don't want to put the secret value directly
        /// in the metric configuration, use the <see cref="EnvironmentVariable"/> property
        /// instead.
        /// </summary>
        public string RawValue { get; set; }

        /// <summary>
        /// The name of an environment variable to get the secret value from.
        /// </summary>
        public string EnvironmentVariable { get; set; }
    }
}
