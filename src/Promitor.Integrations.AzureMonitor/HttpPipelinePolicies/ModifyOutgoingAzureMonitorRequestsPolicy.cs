using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using GuardNet;
using Microsoft.Extensions.Logging;

namespace Promitor.Integrations.AzureMonitor.HttpPipelinePolicies{
    /// <summary>
    ///     Work around to make sure range queries work properly. <see href="https://github.com/Azure/azure-sdk-for-net/issues/40047"/>
    /// </summary>
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
            bool queryModified = false;

            foreach (var param in paramNames)
            {
                if (DateTimeOffset.TryParseExact(query[param], ["MM/dd/yyyy HH:mm:ss zzz", "M/d/yyyy h:mm:ss tt zzz"],  CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dateTime))
                {
                    // Transform to ISO 8601 format (e.g., "2024-09-09T20:46:14")
                    query[param] = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
                    queryModified = true;
                    _logger.LogWarning("Modified {param} to modify parameters to be value {Value}", param, dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"));
                } 
            }
            if (queryModified) {
                message.Request.Uri.Query = query.ToString();
            } else {
                _logger.LogWarning("Failed to modify parameters {Parms}", string.Join("and ", paramNames));
            }
            _logger.LogWarning("Final url is {URI}", message.Request.Uri.ToString());
           
        }
    }
}   

