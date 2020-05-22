using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GuardNet;

namespace Promitor.Agents.Scraper.Discovery
{
    public class ResourceDiscoveryClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ResourceDiscoveryClient(IHttpClientFactory httpClientFactory)
        {
            Guard.NotNull(httpClientFactory,nameof(httpClientFactory));

            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<object>> GetAsync(string resourceCollectionName)
        {
            var client = _httpClientFactory.CreateClient();
            // See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1

            return new List<object>();
        }
    }
}
