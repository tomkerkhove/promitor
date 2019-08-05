using Promitor.Core.Scraping.Configuration.Model.Metrics;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model.Metrics
{
    public class SecretBuilder
    {
        public string RawValue { get; set; }
        public string EnvironmentVariable { get; set; }

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
