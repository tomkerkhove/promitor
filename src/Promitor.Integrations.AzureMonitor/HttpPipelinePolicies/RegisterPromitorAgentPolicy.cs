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

            var id = DetermineApplicationId(azureAuthenticationInfo);
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
            var request = message.Request;
            string agentVersion = Version.Get();
            var promitorUserAgent = ArmUserAgent.Generate(agentVersion, _metricSinkWriter.EnabledMetricSinks);
            request.Headers.Remove(HttpHeader.Names.UserAgent);
            request.Headers.Add(HttpHeader.Names.UserAgent, promitorUserAgent);
            ProcessNext(message, pipeline);
        }

        private string DetermineApplicationId(AzureAuthenticationInfo azureAuthenticationInfo)
        {
            switch (azureAuthenticationInfo.Mode)
            {
                case AuthenticationMode.ServicePrincipal:
                    Guard.NotNullOrWhitespace(azureAuthenticationInfo.IdentityId, nameof(azureAuthenticationInfo.IdentityId));
                    return azureAuthenticationInfo.IdentityId;
                case AuthenticationMode.UserAssignedManagedIdentity:
                    return azureAuthenticationInfo.GetIdentityIdOrDefault("externally-configured-user-assigned-identity");
                case AuthenticationMode.SystemAssignedManagedIdentity:
                    return "system-assigned-identity";
                default:
                    throw new ArgumentOutOfRangeException(nameof(azureAuthenticationInfo.Mode));
            }
        }
    }
}   

