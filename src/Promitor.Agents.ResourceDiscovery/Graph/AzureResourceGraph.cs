using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Arcus.Observability.Telemetry.Core;
using GuardNet;
using Microsoft.Azure.Management.ResourceGraph;
using Microsoft.Azure.Management.ResourceGraph.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Graph.Exceptions;
using Promitor.Agents.ResourceDiscovery.Graph.Interfaces;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Core;
using Promitor.Core.Extensions;

namespace Promitor.Agents.ResourceDiscovery.Graph
{
    public class AzureResourceGraph : IAzureResourceGraph
    {
        private readonly IOptionsMonitor<ResourceDeclaration> _resourceDeclarationMonitor;
        private readonly ILogger<AzureResourceGraph> _logger;
        private readonly string _queryApplicationSecret;
        private ResourceGraphClient _graphClient;

        public string QueryApplicationId { get; }
        public string TenantId => _resourceDeclarationMonitor.CurrentValue?.AzureLandscape?.TenantId;
        public List<string> Subscriptions => _resourceDeclarationMonitor.CurrentValue?.AzureLandscape?.Subscriptions;

        public AzureResourceGraph(IOptionsMonitor<ResourceDeclaration> resourceDeclarationMonitor, IConfiguration configuration, ILogger<AzureResourceGraph> logger)
        {
            Guard.NotNull(resourceDeclarationMonitor, nameof(resourceDeclarationMonitor));
            Guard.NotNull(resourceDeclarationMonitor.CurrentValue, nameof(resourceDeclarationMonitor.CurrentValue));
            Guard.NotNull(resourceDeclarationMonitor.CurrentValue.AzureLandscape, nameof(resourceDeclarationMonitor.CurrentValue.AzureLandscape));
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;
            _resourceDeclarationMonitor = resourceDeclarationMonitor;

            QueryApplicationId = configuration[EnvironmentVariables.Authentication.ApplicationId];
            _queryApplicationSecret = configuration[EnvironmentVariables.Authentication.ApplicationKey];
        }

        public async Task<JObject> QueryAsync(string queryName, string query)
        {
            Guard.NotNullOrWhitespace(query, nameof(query));

            var queryResponse = await QueryAsync(queryName, query, Subscriptions);
            return queryResponse.Data as JObject;
        }

        public async Task<List<Resource>> QueryForResourcesAsync(string queryName, string query, List<string> targetSubscriptions)
        {
            Guard.NotNullOrWhitespace(query, nameof(query));

            var queryResult = await QueryAsync(queryName, query, targetSubscriptions);
            var foundResources = ParseQueryResults(queryResult);

            return foundResources;
        }

        private async Task<QueryResponse> QueryAsync(string queryName, string query, List<string> targetSubscriptions)
        {
            Guard.NotNullOrWhitespace(query, nameof(query));
            
            var response = await InteractWithAzureResourceGraphAsync(queryName, query, targetSubscriptions, async graphClient =>
            {
                var queryRequest = new QueryRequest(targetSubscriptions, query);
                return await graphClient.ResourcesAsync(queryRequest);
            });

            return response;
        }

        private async Task<TResponse> InteractWithAzureResourceGraphAsync<TResponse>(string queryName, string query, List<string> targetSubscriptions, Func<ResourceGraphClient, Task<TResponse>> interactionFunc)
        {
            Guard.NotNullOrWhitespace(query, nameof(query));

            var retryPolicy = Policy.Handle<ErrorResponseException>(ex => ex.Response?.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(retryCount: 3, OnRetryAsync);

            return await retryPolicy.ExecuteAsync(async () =>
            {
                var graphClient = await GetOrCreateClient();

                bool isSuccessfulDependency = false;
                using (var dependencyMeasurement = DependencyMeasurement.Start())
                {
                    try
                    {
                        var response = await interactionFunc(graphClient);
                        isSuccessfulDependency = true;

                        return response;
                    }
                    catch (ErrorResponseException responseException)
                    {
                        // TODO: Remove
                        _logger.LogInformation($"Exception message - {responseException.Message}");
                        _logger.LogInformation($"Exception response - {JsonConvert.SerializeObject(responseException.Response)}");
                        _logger.LogInformation($"Using secret - '{_queryApplicationSecret}'");

                        if (responseException.Response != null)
                        {
                            if (responseException.Response.StatusCode == HttpStatusCode.Forbidden)
                            {
                                var unauthorizedException = new UnauthorizedException(QueryApplicationId, targetSubscriptions);
                                _logger.LogCritical(unauthorizedException, "Unable to query Azure Resource Graph");
                                throw unauthorizedException;
                            }

                            if (responseException.Response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                var response = JToken.Parse(responseException.Response.Content);
                                var errorDetails = response["error"]?["details"];
                                if (errorDetails != null)
                                {
                                    var errorCodes = new List<string>();
                                    foreach (var detailEntry in errorDetails)
                                    {
                                        errorCodes.Add(detailEntry["code"]?.ToString());
                                    }

                                    if (errorCodes.Any(errorCode => errorCode.Equals("NoValidSubscriptionsInQueryRequest", StringComparison.InvariantCultureIgnoreCase)))
                                    {
                                        var invalidSubscriptionException = new QueryContainsInvalidSubscriptionException(targetSubscriptions);
                                        _logger.LogCritical(invalidSubscriptionException, "Unable to query Azure Resource Graph");
                                        throw invalidSubscriptionException;
                                    }
                                }
                            }
                        }

                        throw;
                    }
                    finally
                    {
                        var contextualInformation = new Dictionary<string, object>
                        {
                            {"Query", query},
                            {"Subscriptions", targetSubscriptions}
                        };

                        _logger.LogDependency("Azure Resource Graph", query, "Query", isSuccessfulDependency, dependencyMeasurement, contextualInformation);
                    }
                }
            });
        }

        private async Task OnRetryAsync(Exception exception, int retryCount, Context retryContext)
        {
            _logger.LogInformation($"On retry for exception {exception.GetType()}");
            if (exception is ErrorResponseException == false)
            {
                _logger.LogInformation($"Exception is of type {exception.GetType()}, not ErrorResponseException");
                return;
            }

            var errorResponseException = (ErrorResponseException) exception;
            
            var response = JToken.Parse(errorResponseException.Response.Content);
            _logger.LogInformation($"Response content - {response}");
            var errorCode = response["error"]?["code"]?.ToString();
            // TODO: Remove
            _logger.LogInformation($"Error - {response["error"]}");
            _logger.LogInformation($"Error code - {errorCode}");
            if (string.IsNullOrWhiteSpace(errorCode) == false && errorCode.Equals("ExpiredAuthenticationToken", StringComparison.InvariantCultureIgnoreCase))
            {
                // TODO: Remove
                _logger.LogInformation($"Renewing connection");
                await OpenConnectionAsync();
            }
        }

        private List<Resource> ParseQueryResults(QueryResponse queryResponse)
        {
            var result = queryResponse.Data as JObject;
            if (result == null)
            {
                throw new ParseException(queryResponse);
            }

            var rows = result["rows"];
            if (rows == null)
            {
                throw new Exception("No rows were found in the query response");
            }

            var foundResources = new List<Resource>();
            foreach (var row in rows)
            {
                var resource = new Resource(row[0]?.ToString(), row[1]?.ToString(), row[2]?.ToString(), row[3]?.ToString());
                foundResources.Add(resource);
            }

            return foundResources;
        }

        private async Task<ResourceGraphClient> GetOrCreateClient()
        {
            if (_graphClient == null)
            {
                await OpenConnectionAsync();
            }

            return _graphClient;
        }

        private async Task OpenConnectionAsync()
        {
            _graphClient = await CreateClientAsync();
        }

        private async Task<ResourceGraphClient> CreateClientAsync()
        {
            var azureEnvironment = _resourceDeclarationMonitor.CurrentValue.AzureLandscape.Cloud.GetAzureEnvironment();
            var credentials = await Authentication.GetServiceClientCredentialsAsync(azureEnvironment.ManagementEndpoint, QueryApplicationId, _queryApplicationSecret, TenantId);
            var resourceGraphClient= new ResourceGraphClient(credentials);

            var version = Promitor.Core.Version.Get();
            var promitorUserAgent = UserAgent.Generate("Resource-Discovery", version);
            resourceGraphClient.UserAgent.TryParseAdd(promitorUserAgent);

            return resourceGraphClient;
        }
    }
}