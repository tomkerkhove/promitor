using System;
using System.IO;
using Promitor.Scraper.Configuration.Providers.Interfaces;
using Promitor.Scraper.Model.Configuration;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Promitor.Scraper.Configuration.Providers
{
    public class ScrapeConfigurationProvider : IScrapeConfigurationProvider
    {
        public ScrapeConfiguration GetConfiguration()
        {
            var configurationPath = Environment.GetEnvironmentVariable(EnvironmentVariables.ConfigurationPath);
            var rawConfiguration = File.ReadAllText(configurationPath);
            var input = new StringReader(rawConfiguration);
            var deserializer = GetYamlDeserializer();

            var config = deserializer.Deserialize<ScrapeConfiguration>(input);
            return config;
        }

        private static Deserializer GetYamlDeserializer()
        {
            var builder = new DeserializerBuilder();
            builder.IgnoreUnmatchedProperties();
            builder.WithNamingConvention(new CamelCaseNamingConvention());

            return builder.Build();
        }
    }
}