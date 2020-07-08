using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Logging;

namespace Promitor.Agents.Scraper.Temporary
{
    public class AtlassianStatuspage
    {
        private const string PageId = "y79z9b78ybgs";
        private const string ApiUrl = "https://api.statuspage.io/v1";

        private readonly ILogger<AtlassianStatuspage> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public AtlassianStatuspage(IHttpClientFactory clientFactory, ILogger<AtlassianStatuspage> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task ReportMetricAsync(string id, double value, string apiKey)
        {
            /// Docs: https://developer.statuspage.io/#operation/postPagesPageIdMetricsMetricIdData
            var requestUri = ApiUrl.AppendPathSegment("pages")
                .AppendPathSegment(PageId)
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
