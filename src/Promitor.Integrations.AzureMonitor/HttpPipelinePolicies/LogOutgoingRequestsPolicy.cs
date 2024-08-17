using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using GuardNet;
using Microsoft.Extensions.Logging;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.Azure.Authentication;
using Version = Promitor.Core.Version;

namespace Promitor.Integrations.AzureMonitor.HttpPipelinePolicies{
    public class LogOutgoingRequestsPolicy : HttpPipelinePolicy
    {   
        private readonly ILogger _logger;
        public LogOutgoingRequestsPolicy(ILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));
            _logger = logger;
        }

        public override async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {   
            _logger.LogWarning("URI: {uri}", message.Request.Uri.ToString());
            await ProcessNextAsync(message, pipeline);
        }

        public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            throw new NotSupportedException("Synchronous HTTP request path is not supported");
        }
    }
}   

