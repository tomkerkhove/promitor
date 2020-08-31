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
using Newtonsoft.Json.Linq;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Agents.ResourceDiscovery.Graph.Exceptions;
using Promitor.Agents.ResourceDiscovery.Graph.Model;
using Promitor.Core;
using Promitor.Core.Extensions;

namespace Promitor.Agents.ResourceDiscovery.Graph
{
    public class AzureResourceGraph
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

        public async Task<JObject> QueryAsync(string query)
        {
            Guard.NotNullOrWhitespace(query, nameof(query));

            var graphClient = await GetOrCreateClient();

            bool isSuccessfulDependency = false;
            using (var dependencyMeasurement = DependencyMeasurement.Start())
            {
                try
                {
                    var queryRequest = new QueryRequest(Subscriptions, query);
                    var response = await graphClient.ResourcesAsync(queryRequest);
                    isSuccessfulDependency = true;
                    return response.Data as JObject;
                }
                catch (ErrorResponseException responseException)
                {
                    if (responseException.Response != null)
                    {
                        if (responseException.Response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            var unauthorizedException = new UnauthorizedException(QueryApplicationId, Subscriptions);
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
                                    var invalidSubscriptionException = new QueryContainsInvalidSubscriptionException(Subscriptions);
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
                        {"Subscriptions", Subscriptions}
                    };

                    _logger.LogDependency("Azure Resource Graph", query, "Query", isSuccessfulDependency, dependencyMeasurement, contextualInformation);
                }
            }
        }

        public async Task<List<Resource>> QueryAsync(string query, List<string> targetSubscriptions)
        {
            Guard.NotNullOrWhitespace(query, nameof(query));

            var graphClient = await GetOrCreateClient();

            bool isSuccessfulDependency = false;
            using (var dependencyMeasurement = DependencyMeasurement.Start())
            {
                try
                {
                    var queryRequest = new QueryRequest(targetSubscriptions, query);
                    var response = await graphClient.ResourcesAsync(queryRequest);
                    isSuccessfulDependency = true;

                    var foundResources = ParseQueryResults(response);

                    return foundResources;
                }
                catch (ErrorResponseException responseException)
                {
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
                _graphClient = await CreateClientAsync();
            }

            return _graphClient;
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