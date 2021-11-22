using System;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json.Linq;
using Promitor.Integrations.Sinks.Atlassian.Statuspage;
using Promitor.Tests.Unit.Generators.Config;
using Promitor.Tests.Unit.Stubs;
using Xunit;

namespace Promitor.Tests.Unit.Clients
{
    [Category("Unit")]
    public class AtlassianStatuspageClientTests : UnitTest
    {
        [Fact]
        public async Task ReportMetricAsync_MetricIdAndValueAreProvided_Succeeds()
        {
            // Arrange
            var pageId = BogusGenerator.Name.FirstName();
            var metricId = BogusGenerator.Name.FirstName();
            var metricValue = BogusGenerator.Random.Double();
            var fakeHttpMessageHandler = new HttpMessageHandlerStub();
            var sinkConfiguration = BogusAtlassianStatuspageMetricSinkConfigurationGenerator.GetSinkConfiguration(pageId: pageId);
            var atlassianStatuspageClient = new AtlassianStatuspageClient(httpClient: new HttpClient(fakeHttpMessageHandler), sinkConfiguration, NullLogger<AtlassianStatuspageClient>.Instance);

            // Act
            await atlassianStatuspageClient.ReportMetricAsync(metricId, metricValue);

            // Assert
            var request = fakeHttpMessageHandler.LastRequest;
            Assert.NotNull(request);
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal($"https://api.statuspage.io/v1/pages/{pageId}/metrics/{metricId}/data", request.RequestUri?.ToString());
            Assert.NotNull(request.Content);
            var requestBody = await request.Content.ReadAsStringAsync();
            var requestToken = JToken.Parse(requestBody);
            Assert.Equal(metricValue.ToString(CultureInfo.InvariantCulture), requestToken["data"]?["value"]?.ToString());
        }

        [Fact]
        public async Task ReportMetricAsync_NoMetricIdIsProvided_ThrowsException()
        {
            // Arrange
            var metricValue = BogusGenerator.Random.Double();
            var httpClientMock = new Mock<HttpClient>();
            var sinkConfiguration = BogusAtlassianStatuspageMetricSinkConfigurationGenerator.GetSinkConfiguration();
            var atlassianStatuspageClient = new AtlassianStatuspageClient(httpClient: httpClientMock.Object, sinkConfiguration, NullLogger<AtlassianStatuspageClient>.Instance);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => atlassianStatuspageClient.ReportMetricAsync(id: null, metricValue));
        }

        [Fact]
        public void Constructor_NoHttpClientIsProvided_ThrowsException()
        {
            // Arrange
            var sinkConfiguration = BogusAtlassianStatuspageMetricSinkConfigurationGenerator.GetSinkConfiguration();

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => new AtlassianStatuspageClient(httpClient: null, sinkConfiguration, NullLogger<AtlassianStatuspageClient>.Instance));
        }

        [Fact]
        public void Constructor_NoSinkConfigurationIsProvided_ThrowsException()
        {
            // Arrange
            var httpClientMock = new Mock<HttpClient>();

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => new AtlassianStatuspageClient(httpClientMock.Object, sinkConfiguration: null, NullLogger<AtlassianStatuspageClient>.Instance));
        }

        [Fact]
        public void Constructor_NoLoggerIsProvided_ThrowsException()
        {
            // Arrange
            var httpClientMock = new Mock<HttpClient>();
            var sinkConfiguration = BogusAtlassianStatuspageMetricSinkConfigurationGenerator.GetSinkConfiguration();

            // Act & Assert
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => new AtlassianStatuspageClient(httpClientMock.Object, sinkConfiguration, logger: null));
        }
    }
}
