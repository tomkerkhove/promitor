﻿using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Promitor.Core.Configuration.Model;
using Promitor.Scraper.Tests.Unit.Generators.Config;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Configuration
{
    [Category("Unit")]
    public class RuntimeConfigurationUnitTest
    {
        [Fact]
        public async Task RuntimeConfiguration_CompleteConfiguration_Succeeds()
        {
            // Arrange
            var fileUri = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .GenerateAsync();

            var configuration = GetRuntimeConfiguration(fileUri);
            
            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
        }

        public static IConfigurationRoot GetRuntimeConfiguration(string fileUri)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(fileUri))
                .AddYamlFile(Path.GetFileName(fileUri))
                .Build();
        }
    }
}
