using System;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using GuardNet;
using Promitor.Core.Metrics.Sinks;
using Promitor.Integrations.Azure.Authentication;
using Version = Promitor.Core.Version;

namespace Promitor.Integrations.AzureMonitor.HttpPipelinePolicies{
    public class RegisterPromitorAgentPolicy : HttpPipelinePolicy
    {   
        private readonly MetricSinkWriter _metricSinkWriter;
        public RegisterPromitorAgentPolicy(string tenantId, string subscriptionId, AzureAuthenticationInfo azureAuthenticationInfo, MetricSinkWriter metricSinkWriter)
        {
            Guard.NotNullOrWhitespace(tenantId, nameof(tenantId));
            Guard.NotNullOrWhitespace(subscriptionId, nameof(subscriptionId));
            Guard.NotNull(metricSinkWriter, nameof(metricSinkWriter));
            Guard.NotNull(azureAuthenticationInfo, nameof(azureAuthenticationInfo));

            _metricSinkWriter = metricSinkWriter;
        }

        public override async ValueTask ProcessAsync(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {   
            var request = message.Request;
            string agentVersion = Version.Get();
            var promitorUserAgent = ArmUserAgent.Generate(agentVersion, _metricSinkWriter.EnabledMetricSinks);
            request.Headers.Remove(HttpHeader.Names.UserAgent);
            request.Headers.Add(HttpHeader.Names.UserAgent, promitorUserAgent);
            await ProcessNextAsync(message, pipeline);
        }

        public override void Process(HttpMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            throw new NotSupportedException("Synchronous HTTP request path is not supported");
        }
    }
}   

