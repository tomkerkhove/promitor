using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;

namespace Promitor.Integrations.AzureMonitor.Logging
{
    public class AzureMonitorIntegrationLogger : IServiceClientTracingInterceptor
    {
        private readonly ILogger _logger;

        public AzureMonitorIntegrationLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Information(string message)
        {
            _logger.Log(LogLevel.Debug, message);
        }

        public void TraceError(string invocationId, Exception exception)
        {
            _logger.LogError("Exception in {0}: {1}", invocationId, exception);
        }

        public void ReceiveResponse(string invocationId, HttpResponseMessage response)
        {
        }

        public void SendRequest(string invocationId, HttpRequestMessage request)
        {
        }

        public void Configuration(string source, string name, string value)
        {
        }

        public void EnterMethod(string invocationId, object instance, string method, IDictionary<string, object> parameters)
        {
        }

        public void ExitMethod(string invocationId, object returnValue)
        {
        }
    }
}