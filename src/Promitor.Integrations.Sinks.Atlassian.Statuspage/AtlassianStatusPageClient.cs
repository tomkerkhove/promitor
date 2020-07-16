using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Integrations.Sinks.Atlassian.Statuspage.Configuration;

namespace Promitor.Integrations.Sinks.Atlassian.Statuspage
{
    public class AtlassianStatuspageClient : IAtlassianStatuspageClient
    {
        private const string MetricRequestFormat = "{{\"data\": {{\"timestamp\": {0},\"value\": {1}}}}}";
        private const string ApiUrl = "https://api.statuspage.io/v1";

        private readonly IOptionsMonitor<AtlassianStatusPageSinkConfiguration> _sinkConfiguration;
        private readonly ILogger<AtlassianStatuspageClient> _logger;
        private readonly HttpClient _httpClient;

        public AtlassianStatuspageClient(HttpClient httpClient, IOptionsMonitor<AtlassianStatusPageSinkConfiguration> sinkConfiguration, ILogger<AtlassianStatuspageClient> logger)
        {
            Guard.NotNull(httpClient, nameof(httpClient));
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(sinkConfiguration, nameof(sinkConfiguration));
            Guard.NotNull(sinkConfiguration.CurrentValue, nameof(sinkConfiguration.CurrentValue));

            _sinkConfiguration = sinkConfiguration;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task ReportMetricAsync(string id, double value)
        {
            Guard.NotNullOrWhitespace(id, nameof(id));

            var pageId = _sinkConfiguration.CurrentValue.PageId;
            
            // Docs: https://developer.statuspage.io/#operation/postPagesPageIdMetricsMetricIdData
            var requestUri = ApiUrl.AppendPathSegment("pages")
                .AppendPathSegment(pageId)
                .AppendPathSegment("metrics")
                .AppendPathSegment(id)
                .AppendPathSegment("data");

            var measurementTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(string.Format(MetricRequestFormat, measurementTime, value), Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode == false)
            {
                var message = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to report metric. Details: {message}");
            }
        }
    }
}
