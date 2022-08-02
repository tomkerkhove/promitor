using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using GuardNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Promitor.Tests.Integration.Clients
{
    public class AgentClient
    {
        protected IConfiguration Configuration { get; }
        protected HttpClient HttpClient { get; }
        protected string AgentName{ get; }
        protected ILogger Logger { get; }

        public AgentClient(string agentName, string baseUrlConfigKey, IConfiguration configuration, ILogger logger)
        {
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(logger, nameof(logger));

            var baseUrl = configuration[baseUrlConfigKey];
            logger.LogInformation("Base URL for {AgentName} is '{Url}'", agentName, baseUrl);

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            Configuration = configuration;
            AgentName = agentName;
            Logger = logger;
        }

        public async Task<HttpResponseMessage> GetHealthAsync()
        {
            return await GetAsync("/api/v1/health");
        }

        public async Task<HttpResponseMessage> GetSystemInfoAsync()
        {
            return await GetAsync("/api/v1/system");
        }

        protected async Task<HttpResponseMessage> GetAsync(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var stopwatch = Stopwatch.StartNew();
            var response = await HttpClient.SendAsync(request);
            stopwatch.Stop();

            var context = new Dictionary<string, object>();
            try
            {
                await response.Content.ReadAsStringAsync();
                // TODO: Uncomment for full payload during troubleshooting
                //context.Add("Body", rawResponse);
            }
            finally
            {
                Logger.LogRequest(request, response, stopwatch.Elapsed, context);
            }

            return response;
        }

        protected JsonSerializerSettings GetJsonSerializerSettings()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore
            };
            return jsonSerializerSettings;
        }

        protected TResponse GetDeserializedResponse<TResponse>(string rawResponse)
        {
            var jsonSerializerSettings = GetJsonSerializerSettings();
            return JsonConvert.DeserializeObject<TResponse>(rawResponse, jsonSerializerSettings);
        }
    }
}