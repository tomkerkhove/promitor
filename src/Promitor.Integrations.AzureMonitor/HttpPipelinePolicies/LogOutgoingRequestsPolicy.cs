using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using GuardNet;
using Microsoft.Extensions.Logging;

namespace Promitor.Integrations.AzureMonitor.HttpPipelinePolicies{
    public class ModifyOutgoingAzureMonitorRequestsPolicy : HttpPipelinePolicy
    {   
        private readonly ILogger _logger;
        public ModifyOutgoingAzureMonitorRequestsPolicy(ILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));
            _logger = logger;
        }

        public override async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {   
            ModifyDateTimeParam(["starttime", "endtime"], message);
            _logger.LogWarning("Modified URI: {uri}", message.Request.Uri.ToString());
            await ProcessNextAsync(message, pipeline);
        }

        public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            throw new NotSupportedException("Synchronous HTTP request path is not supported");
        }

        private void ModifyDateTimeParam(List<string> paramNames, HttpMessage message) 
        {
            // Modify the request URL by updating or adding a query parameter
            var uriBuilder = new UriBuilder(message.Request.Uri.ToString());
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var param in paramNames)
            {
                _logger.LogWarning("Original URI param {param} is {value}", param, query[param]);

                if (DateTimeOffset.TryParseExact(query[param], "MM/dd/yyyy HH:mm:ss zzz",  CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTime))
                {
                    // Transform to ISO 8601 format (e.g., "2024-09-09T20:46:14")
                    query[param] = dateTime.ToString("o", CultureInfo.InvariantCulture);
                    _logger.LogWarning("Modified URI param {param} to be {value}", param, query[param]);
                    // Update the message with the modified URI
                }
            }
             message.Request.Uri.Query = query.ToString();
        }
    }
}   

