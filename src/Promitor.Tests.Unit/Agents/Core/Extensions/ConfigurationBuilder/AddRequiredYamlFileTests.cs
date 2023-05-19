namespace Promitor.Tests.Unit.Agents.Core.Extensions.ConfigurationBuilder
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using System.IO;

    using Xunit;
    using System;
    using NetEscapades.Configuration.Yaml;
    using Promitor.Agents.Core.Extensions;

    public class AddRequiredYamlFileTests : IDisposable
    {
        private readonly List<string> _tempFiles = new();

        [Fact]
        public void AddRequiredYamlFile_FileExists_AddsFile()
        {
            // Arrange
            var yamlFile = CreateYamlFile();
            var builder = new ConfigurationBuilder();
            builder.AddRequiredYamlFile(yamlFile.FullName, true);

            // Act
            var source = Assert.IsType<YamlConfigurationSource>(builder.Sources[0]);

            // Assert
            Assert.Equal(yamlFile.Name, source.Path);
        }

        [Fact]
        public void AddRequiredYamlFile_FileDoesNotExist_ThrowsException()
        {
            // Arrange
            const string configurationFilename = "/opt/promitor/runtime.yaml";
            var builder = new ConfigurationBuilder();

            // Act
            var exception = Assert.Throws<ConfigurationFileNotFoundException>(
                () => builder.AddRequiredYamlFile(configurationFilename, true));

            // Assert
            Assert.Equal(configurationFilename, exception.Path);
        }

        public void Dispose()
        {
            foreach (var file in _tempFiles)
            {
                File.Delete(file);
            }

            _tempFiles.Clear();
        }

        private FileInfo CreateYamlFile()
        {
            var yamlFile = Path.GetTempFileName();
            using (var writer = new StreamWriter(yamlFile))
            {
                writer.WriteLine("name: promitor");
            }

            _tempFiles.Add(yamlFile);

            return new FileInfo(yamlFile);
        }
  }
}