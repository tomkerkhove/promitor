using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Promitor.Integrations.AzureMonitor.Extensions
{
    public static class AzureMonitorMetadataTasks 
    {
        public static async Task<IReadOnlyList<MetricDefinition>> GetAndCacheMetricDefinitionsAsync(this MetricsQueryClient metricsQueryClient, string resourceId, string metricNamespace, IMemoryCache resourceMetricDefinitionMemoryCache, TimeSpan cacheDuration)
        {
            // Get cached metric definitions
            if (resourceMetricDefinitionMemoryCache.TryGetValue(resourceId, out IReadOnlyList<MetricDefinition> metricDefinitions))
            {
                return metricDefinitions;
            }
            var metricsDefinitions = new List<MetricDefinition>(); 
            await foreach (var definition in metricsQueryClient.GetMetricDefinitionsAsync(resourceId, metricNamespace))
            {
                metricsDefinitions.Add(definition);
            }
            
            // Get from API and cache it
            resourceMetricDefinitionMemoryCache.Set(resourceId, metricsDefinitions, cacheDuration);

            return metricsDefinitions;
        }

        public static async Task<List<string>> GetAndCacheMetricNamespacesAsync(this MetricsQueryClient metricsQueryClient, string resourceId, IMemoryCache resourceMetricDefinitionMemoryCache, TimeSpan cacheDuration)
        {
            // Get cached metric namespaces 
            var namespaceKey = $"{resourceId}_namespace";
            if (resourceMetricDefinitionMemoryCache.TryGetValue(namespaceKey, out List<string> metricNamespaces))
            {
                return metricNamespaces;
            }
            var foundMetricNamespaces = new List<string>(); 
            await foreach (var metricNamespace in metricsQueryClient.GetMetricNamespacesAsync(resourceId))
            {
                foundMetricNamespaces.Add(metricNamespace.FullyQualifiedName);
            }
            
            // Get from API and cache it
            resourceMetricDefinitionMemoryCache.Set(namespaceKey, foundMetricNamespaces, cacheDuration);

            return foundMetricNamespaces;
        }
    }
}