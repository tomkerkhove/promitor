using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Configuration;
using Promitor.Core.Configuration.Model;
using Promitor.Scraper.Tests.Unit.Generators.Config;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Configuration
{
    [Category("Unit")]
    public class RuntimeConfigurationUnitTes
    {
        private readonly Faker faker = new Faker();

        [Fact]
        public async Task RuntimeConfiguration_CompleteConfiguration_Succeeds()
        {
            // Arrange
            var bogusHttpPort = faker.Random.Int();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration(httpPort: bogusHttpPort)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Server);
            Assert.Equal(bogusHttpPort, runtimeConfiguration.Server.HttpPort);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasDifferentHttpPort_Succeeds()
        {
            // Arrange
            var bogusHttpPort = faker.Random.Int();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration(httpPort: bogusHttpPort)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Server);
            Assert.Equal(bogusHttpPort, runtimeConfiguration.Server.HttpPort);
        }
    }
}