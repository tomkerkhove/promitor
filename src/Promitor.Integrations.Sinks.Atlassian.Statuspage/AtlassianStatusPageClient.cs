using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Core;
using Promitor.Integrations.Sinks.Atlassian.Statuspage.Configuration;

namespace Promitor.Integrations.Sinks.Atlassian.Statuspage
{
    public class AtlassianStatuspageClient
    {
        private const string ApiUrl = "https://api.statuspage.io/v1";

        private readonly IOptionsMonitor<AtlassianStatusPageSinkConfiguration> _sinkConfiguration;
        private readonly ILogger<AtlassianStatuspageClient> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public AtlassianStatuspageClient(IHttpClientFactory clientFactory, IConfiguration configuration, IOptionsMonitor<AtlassianStatusPageSinkConfiguration> sinkConfiguration, ILogger<AtlassianStatuspageClient> logger)
        {
            Guard.NotNull(clientFactory, nameof(clientFactory));
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(sinkConfiguration, nameof(sinkConfiguration));
            Guard.NotNull(sinkConfiguration.CurrentValue, nameof(sinkConfiguration.CurrentValue));

            _sinkConfiguration = sinkConfiguration;
            _clientFactory = clientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task ReportMetricAsync(string id, double value)
        {
            var pageId = _sinkConfiguration.CurrentValue.PageId;
            var apiKey = _configuration[EnvironmentVariables.Integrations.AtlassianStatuspage.ApiKey];
            /// Docs: https://developer.statuspage.io/#operation/postPagesPageIdMetricsMetricIdData
            var requestUri = ApiUrl.AppendPathSegment("pages")
                .AppendPathSegment(pageId)
                .AppendPathSegment("metrics")
                .AppendPathSegment(id)
                .AppendPathSegment("data");

            var time = DateTimeOffset.Now.ToUnixTimeSeconds();
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent($"{{\"data\": {{\"timestamp\": {time},\"value\": {value}}}}}", Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"OAuth {apiKey}");
            request.Headers.Add("User-Agent", "Sandbox");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode == false)
            {
                var message = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to report metric. Details: {message}");
            }
        }
    }
}
