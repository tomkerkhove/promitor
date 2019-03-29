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
        private int _subscriptionReadLimit;
        public AzureMonitorHandler(ILogger logger, ref int subscriptionReadLimit)
        {
            _logger = logger;
            _subscriptionReadLimit = subscriptionReadLimit;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.Headers.Contains("x-ms-ratelimit-remaining-subscription-reads"))
            {
                var remaining = response.Headers.GetValues("x-ms-ratelimit-remaining-subscription-reads").FirstOrDefault();
                _subscriptionReadLimit = Convert.ToInt16(remaining);
            }

            if ((int)response.StatusCode == 429)
            {
                _logger.LogWarning("Azure subscription rate limit reached.");
            }

            return response;
        }
    }
}
