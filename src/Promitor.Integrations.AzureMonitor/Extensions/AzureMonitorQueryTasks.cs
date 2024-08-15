using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Core;
using Promitor.Integrations.AzureMonitor.Configuration;
using Promitor.Integrations.AzureMonitor.Exceptions;

namespace Promitor.Integrations.AzureMonitor.Extensions
{
    public static class AzureMonitorQueryTasks 
    {
        public static async Task<MetricResult> GetRelevantMetricSingleResource(this MetricsQueryClient metricsQueryClient, string resourceId, string metricName, MetricAggregationType metricAggregation, TimeSpan metricInterval,
            string metricFilter, List<string> metricDimensions, int? metricLimit, DateTime recordDateTime, IOptions<AzureMonitorIntegrationConfiguration> azureMonitorIntegrationConfiguration)
        {   
            MetricsQueryOptions queryOptions;
            var querySizeLimit = metricLimit ?? Defaults.MetricDefaults.Limit;
            var historyStartingFromInHours = azureMonitorIntegrationConfiguration.Value.History.StartingFromInHours;
            var filter = BuildFilter(metricDimensions, metricFilter);

            if (!string.IsNullOrEmpty(filter))
            {
                queryOptions = new MetricsQueryOptions {
                    Aggregations = {
                        metricAggregation
                    }, 
                    Granularity = metricInterval,
                    Filter = filter,
                    Size = querySizeLimit, 
                    TimeRange= new QueryTimeRange(new DateTimeOffset(recordDateTime.AddHours(-historyStartingFromInHours)), new DateTimeOffset(recordDateTime))
                };
            } 
            else 
            {
                queryOptions = new MetricsQueryOptions {
                    Aggregations= {
                        metricAggregation
                    }, 
                    Granularity = metricInterval,
                    Size = querySizeLimit, 
                    TimeRange= new QueryTimeRange(new DateTimeOffset(recordDateTime.AddHours(-historyStartingFromInHours)), new DateTimeOffset(recordDateTime))
                };
            }
            
            var metricsQueryResponse = await metricsQueryClient.QueryResourceAsync(resourceId, [metricName], queryOptions);
            return GetRelevantMetricResultOrThrow(metricsQueryResponse.Value, metricName); 
        }

        public static async Task<List<MetricResult>> GetRelevantMetricForResourçes(this MetricsClient metricsClient, List<string> resourceIds, string metricName, string metricNamespace, MetricAggregationType metricAggregation, TimeSpan metricInterval,
            string metricFilter, List<string> metricDimensions, int? metricLimit, DateTime recordDateTime, IOptions<AzureMonitorIntegrationConfiguration> azureMonitorIntegrationConfiguration, ILogger logger)
        {   
            MetricsQueryResourcesOptions queryOptions;
            var querySizeLimit = metricLimit ?? Defaults.MetricDefaults.Limit;
            var historyStartingFromInHours = azureMonitorIntegrationConfiguration.Value.History.StartingFromInHours;
            var filter = BuildFilter(metricDimensions, metricFilter);
            List<ResourceIdentifier> resourceIdentifiers = resourceIds.Select(id => new ResourceIdentifier(id)).ToList(); 

            if (!string.IsNullOrEmpty(filter))
            {
                queryOptions = new MetricsQueryResourcesOptions {
                    Aggregations = { metricAggregation.ToString() }, 
                    Granularity = metricInterval,
                    Filter = filter,
                    Size = querySizeLimit, 
                    TimeRange= new QueryTimeRange(new DateTimeOffset(recordDateTime.AddHours(-historyStartingFromInHours)), new DateTimeOffset(recordDateTime))
                };
            } 
            else 
            {
                queryOptions = new MetricsQueryResourcesOptions {
                    Aggregations = { metricAggregation.ToString() },
                    Granularity = metricInterval,
                    Size = querySizeLimit, 
                    TimeRange= new QueryTimeRange(new DateTimeOffset(recordDateTime.AddHours(-historyStartingFromInHours)), new DateTimeOffset(recordDateTime))
                };
            }
            logger.LogWarning("Batch query options: {Options}", queryOptions);
            
            var metricsBatchQueryResponse = await metricsClient.QueryResourcesAsync(resourceIdentifiers, [metricName], metricNamespace, queryOptions);
            var metricsQueryResults = metricsBatchQueryResponse.Value;
            logger.LogWarning("Got response");
            return metricsQueryResults.Values
                .Select(result => GetRelevantMetricResultOrThrow(result, metricName))
                .ToList();
        }

        private static string BuildFilter(List<String> metricDimensions, string metricFilter)
        {
            var filterDictionary = new Dictionary<string, string>();
            metricDimensions.ForEach(metricDimension => filterDictionary.Add(metricDimension, "'*'"));
            
            if (string.IsNullOrWhiteSpace(metricFilter) == false) {
                var filterConditions = metricFilter.Split(" and ").ToList();
                foreach (string condition in filterConditions) 
                {
                    string[] parts = condition.Split(" eq ", StringSplitOptions.None);
                    if (filterDictionary.ContainsKey(parts[0]))
                    {
                        filterDictionary[parts[0]] = parts[1];
                    } 
                    else 
                    {
                        filterDictionary.Add(parts[0].Trim(), parts[1]);
                    }
                }
            }

            if (filterDictionary.Count > 0) 
            {
                return string.Join(" and ", filterDictionary.Select(kvp => $"{kvp.Key} eq {kvp.Value}"));
            }
            return null;
        }

        private static MetricResult GetRelevantMetricResultOrThrow(MetricsQueryResult metricsQueryResult, string metricName)
        {
            var relevantMetric = metricsQueryResult.Metrics.SingleOrDefault(var => var.Name.ToUpper() == metricName.ToUpper());
            if (relevantMetric == null)
            {
                throw new MetricNotFoundException(metricName);
            }

            return relevantMetric;
        }
    }
}