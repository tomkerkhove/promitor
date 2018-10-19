using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Monitor.Fluent;
using Microsoft.Azure.Management.Monitor.Fluent.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using GuardNet;
using Promitor.Integrations.AzureMonitor.Exceptions;

namespace Promitor.Integrations.AzureMonitor
{
    public class AzureMonitorClient
    {
        private readonly IAzure _authenticatedAzureSubscription;
        private readonly AzureCredentialsFactory _azureCredentialsFactory = new AzureCredentialsFactory();

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="tenantId">Id of the tenant that owns the Azure subscription</param>
        /// <param name="subscriptionId">Id of the Azure subscription</param>
        /// <param name="applicationId">Id of the Azure AD application used to authenticate with Azure Monitor</param>
        /// <param name="applicationSecret">Secret to authenticate with Azure Monitor for the specified Azure AD application</param>
        public AzureMonitorClient(string tenantId, string subscriptionId, string applicationId,
            string applicationSecret)
        {
            Guard.NotNullOrWhitespace(tenantId, nameof(tenantId));
            Guard.NotNullOrWhitespace(subscriptionId, nameof(subscriptionId));
            Guard.NotNullOrWhitespace(applicationId, nameof(applicationId));
            Guard.NotNullOrWhitespace(applicationSecret, nameof(applicationSecret));

            var credentials = _azureCredentialsFactory.FromServicePrincipal(applicationId, applicationSecret, tenantId, AzureEnvironment.AzureGlobalCloud);

            _authenticatedAzureSubscription = Azure.Authenticate(credentials).WithSubscription(subscriptionId);
        }

        /// <summary>
        ///     Queries Azure Monitor to get the latest value for a specific metric
        /// </summary>
        /// <param name="metricName">Name of the metric</param>
        /// <param name="metricAggregation">Aggregation for the metric to use</param>
        /// <param name="resourceId">Id of the resource to query</param>
        /// <param name="metricFilter">Optional filter to filter out metrics</param>
        /// <returns>Latest representation of the metric</returns>
        public async Task<double> QueryMetricAsync(string metricName, AggregationType metricAggregation,
            string resourceId, MetricType metricType, string metricFilter = null)
        {
            Guard.NotNullOrWhitespace(metricName, nameof(metricName));
            Guard.NotNullOrWhitespace(resourceId, nameof(resourceId));

            // Get all metrics
            var metricsDefinitions = await _authenticatedAzureSubscription.MetricDefinitions.ListByResourceAsync(resourceId);
            var metricDefinition = metricsDefinitions.SingleOrDefault(definition => definition.Name.Value.ToUpper() == metricName.ToUpper());
            if (metricDefinition == null)
            {
                throw new MetricNotFoundException(metricName);
            }

            double result = 0.0D;
            if (metricType == MetricType.Capacity || metricType == MetricType.Transactional)
            {
                var queryRunner = new AzureMonitorQueryRunner();
                var readDataNew = new DataPointReader(queryRunner);
                result = await readDataNew.ReadDataPoint(metricName, metricAggregation, metricFilter, metricDefinition, metricType);
            }
            else
            {
                TypelessDataPointReader rdo = new TypelessDataPointReader();
                result = await rdo.ReadDataPoint(metricName, metricAggregation, metricFilter, metricDefinition, metricType);
            }

            return result;
        }
    }
}