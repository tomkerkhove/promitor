using System.Collections.Generic;
using GuardNet;
using Promitor.Core.Metrics;

namespace Promitor.Core
{
    public class ScrapeResult
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="subscriptionId">Subscription that contains the resource that was scraped</param>
        /// <param name="resourceGroupName">Resource group name that contains the resource that was scraped</param>
        /// <param name="resourceUri">Uri of the resource that was scraped</param>
        /// <param name="metricValues">Collection of metrics that were found</param>
        public ScrapeResult(string subscriptionId, string resourceGroupName, string resourceUri, List<MeasuredMetric> metricValues) : this(subscriptionId, resourceGroupName, string.Empty, resourceUri, metricValues, new Dictionary<string, string>())
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="subscriptionId">Subscription that contains the resource that was scraped</param>
        /// <param name="resourceGroupName">Resource group name that contains the resource that was scraped</param>
        /// <param name="instanceName">Name of the resource that is being scraped</param>
        /// <param name="resourceUri">Uri of the resource that was scraped</param>
        /// <param name="metricValues">Collection of metrics that were found</param>
        /// <param name="customLabels">A collection of custom labels to add to the scraping result</param>
        public ScrapeResult(string subscriptionId, string resourceGroupName, string instanceName, string resourceUri, List<MeasuredMetric> metricValues, Dictionary<string, string> customLabels)
        {
            //Todo resourceUri is null when resourceType=logAnalytics
            Guard.NotNullOrEmpty(subscriptionId, nameof(subscriptionId));
            Guard.NotNullOrEmpty(resourceGroupName, nameof(resourceGroupName));
            // Guard.NotNullOrEmpty(resourceUri, nameof(resourceUri));
            Guard.NotNull(customLabels, nameof(customLabels));

            SubscriptionId = subscriptionId;
            ResourceGroupName = resourceGroupName;
            ResourceUri = resourceUri;
            MetricValues = metricValues;

            Labels.Add("resource_group", resourceGroupName);
            Labels.Add("subscription_id", subscriptionId);
            Labels.Add("resource_uri", resourceUri);

            if (string.IsNullOrWhiteSpace(instanceName) == false)
            {
                InstanceName = instanceName;
                Labels.Add("instance_name", instanceName);
            }

            foreach (KeyValuePair<string, string> customLabel in customLabels)
            {
                Labels.TryAdd(customLabel.Key, customLabel.Value);
            }
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="subscriptionId">Subscription that contains the resource that was scraped</param>
        /// <param name="resourceGroupName">Resource group name that contains the resource that was scraped</param>
        /// <param name="instanceName">Name of the resource that is being scraped</param>
        /// <param name="resourceUri">Uri of the resource that was scraped</param>
        /// <param name="metricValues">Collection of metrics that were found</param>
        public ScrapeResult(string subscriptionId, string resourceGroupName, string instanceName, string resourceUri, List<MeasuredMetric> metricValues) : this(subscriptionId, resourceGroupName, instanceName, resourceUri, metricValues, new Dictionary<string, string>())
        {
        }

        /// <summary>
        ///     Subscription that contains the resource that was scraped
        /// </summary>
        public string SubscriptionId { get; }

        /// <summary>
        ///     Resource group name that contains the resource that was scraped
        /// </summary>
        public string ResourceGroupName { get; }

        /// <summary>
        ///     Name of the resource that is being scraped
        /// </summary>
        public string InstanceName { get; }

        /// <summary>
        ///     Uri of the resource that was scraped
        /// </summary>
        public string ResourceUri { get; }

        /// <summary>
        ///     Value of the metric that was found
        /// </summary>
        public List<MeasuredMetric> MetricValues { get; }

        /// <summary>
        /// Labels that are related to the metric
        /// </summary>
        public Dictionary<string, string> Labels { get; } = new();
    }
}