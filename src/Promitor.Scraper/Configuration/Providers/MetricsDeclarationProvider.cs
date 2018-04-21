using System;
using System.IO;
using Promitor.Scraper.Configuration.Providers.Interfaces;
using Promitor.Scraper.Model.Configuration;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Promitor.Scraper.Configuration.Providers
{
    public class MetricsDeclarationProvider : IMetricsDeclarationProvider
    {
        public virtual MetricsDeclaration Get()
        {
            var rawMetricsDeclaration = GetSerializedDeclaration();
            var input = new StringReader(rawMetricsDeclaration);
            var deserializer = GetYamlDeserializer();

            var config = deserializer.Deserialize<MetricsDeclaration>(input);
            return config;
        }

        public virtual string GetSerializedDeclaration()
        {
            var scrapingConfigurationPath = Environment.GetEnvironmentVariable(EnvironmentVariables.ConfigurationPath);
            if (string.IsNullOrWhiteSpace(scrapingConfigurationPath))
            {
                Console.WriteLine($"No scraping configuration path was specified, falling back to default '{Constants.Defaults.MetricsDeclarationPath}'...");
                scrapingConfigurationPath = Constants.Defaults.MetricsDeclarationPath;
            }

            var rawMetricsDeclaration = File.ReadAllText(scrapingConfigurationPath);
            return rawMetricsDeclaration;
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