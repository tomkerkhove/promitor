﻿using System;
using System.IO;
using Promitor.Scraper.Host.Configuration.Model;
using Promitor.Scraper.Host.Configuration.Providers.Interfaces;
using Promitor.Scraper.Host.Serialization;

namespace Promitor.Scraper.Host.Configuration.Providers
{
    public class MetricsDeclarationProvider : IMetricsDeclarationProvider
    {
        public virtual MetricsDeclaration Get()
        {
            var rawMetricsDeclaration = GetSerializedDeclaration();
            var input = new StringReader(rawMetricsDeclaration);
            var deserializer = YamlSerialization.CreateDeserializer();

            var config = deserializer.Deserialize<MetricsDeclaration>(input);
            return config;
        }

        public virtual string GetSerializedDeclaration()
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