using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Configuration.Serialization;

namespace Promitor.Core.Scraping.Configuration.Providers
{
    public class MetricsDeclarationProvider : IMetricsDeclarationProvider
    {
        private readonly ConfigurationSerializer configurationSerializer;

        public MetricsDeclarationProvider(ILogger logger)
        {
            configurationSerializer = new ConfigurationSerializer(logger);
        }

        public virtual MetricsDeclaration Get()
        {
            var rawMetricsDeclaration = ReadRawDeclaration();

            var config = configurationSerializer.Deserialize(rawMetricsDeclaration);
            return config;
        }

        public virtual string ReadRawDeclaration()
        {
            var scrapingConfigurationPath = Environment.GetEnvironmentVariable(EnvironmentVariables.Configuration.Path);
            if (string.IsNullOrWhiteSpace(scrapingConfigurationPath))
            {
                Console.WriteLine($"No scraping configuration path was specified, falling back to default '{Constants.Defaults.MetricsDeclarationPath}'...");
                scrapingConfigurationPath = Constants.Defaults.MetricsDeclarationPath;
            }

            var rawMetricsDeclaration = File.ReadAllText(scrapingConfigurationPath);
            return rawMetricsDeclaration;
        }
    }
}