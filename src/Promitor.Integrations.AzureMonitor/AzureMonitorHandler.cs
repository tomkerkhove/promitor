using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promitor.Integrations.AzureMonitor
{
    public class AzureMonitorHandler : DelegatingHandlerBase
    {
        private readonly ILogger _logger;
        public AzureMonitorHandler(ILogger logger)
        {
            _logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.Headers.Contains("x-ms-ratelimit-remaining-subscription-reads"))
            {
                var remaining = response.Headers.GetValues("x-ms-ratelimit-remaining-subscription-reads").FirstOrDefault();

                // add singleton call

                Console.WriteLine(remaining);
            }

            if ((int)response.StatusCode == 429)
            {
                _logger.LogWarning("Azure subscription rate limit reached.");
            }

            return response;
        }
    }
}
