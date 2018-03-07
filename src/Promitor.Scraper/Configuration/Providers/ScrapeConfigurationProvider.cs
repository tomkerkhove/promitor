using System;
using System.IO;
using Promitor.Scraper.Configuration.Model;
using Promitor.Scraper.Configuration.Providers.Interfaces;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Promitor.Scraper.Configuration.Providers
{
    public class ScrapeConfigurationProvider : IScrapeConfigurationProvider
    {
        public ScrapeConfiguration GetConfiguration()
        {
            var configurationPath = Environment.GetEnvironmentVariable(EnvironmentVariables.ConfigurationPath);
            var rawConfiguragion = File.ReadAllText(configurationPath);
            var input = new StringReader(rawConfiguragion);
            var deserializer = GetYamlDeserializer();

            var config = deserializer.Deserialize<ScrapeConfiguration>(input);
            return config;
        }

        private static Deserializer GetYamlDeserializer()
        {
            var builder = new DeserializerBuilder();
            builder.WithNamingConvention(new CamelCaseNamingConvention());

            return builder.Build();
        }
    }
}