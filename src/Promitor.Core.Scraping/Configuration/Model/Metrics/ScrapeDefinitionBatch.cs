using System;
using System.Collections.Generic;
using Promitor.Core.Contracts;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    /// <summary>
    /// Defines properties of a batch of scrape definitions 
    /// </summary>
    public class ScrapeDefinitionBatchProperties : IEquatable<ScrapeDefinitionBatchProperties>
    { 

    }

    /// <summary>
    /// Configuration about the Azure Monitor metric to scrape
    /// </summary>
    public AzureMetricConfiguration AzureMetricConfiguration { get; }


    /// <summary>
    /// The scraping model.
    /// </summary>
    public Scraping Scraping { get; }

    /// <summary>
    /// The Azure subscription to get the metric from. This should be used instead of using
    /// the SubscriptionId from <see cref="Resource"/> because this property will contain
    /// the global subscription id if none is overridden at the resource level.
    /// </summary>
    public string SubscriptionId { get; }

    /// <summary>
    /// The Azure resource type shared by all scrape definitions in the batch
    /// </summary>
    public ResourceType ResourceType { get; }

    public TimeSpan AggregationInterval{ get; }

    public override int GetHashCode()
    {
        return this.BuildBatchHashKey().GetHashCode(); 
    }

    /// <summary>
    /// Builds a namespaced string key to satisfy batch restrictions, in the format of
    /// <AzureMetricAndDimensions>_<SubscriptionId>_<ResourceType>_<AggregationInterval> 
    /// </summary>
    private string BuildBatchHashKey()
    {
        return string.Join("_",  [List.ofAzureMetricConfiguration.ToUniqueStringRepresentation(), SubscriptionId, ResourceType.ToString(), AggregationInterval.ToString]);
    }

    /// <summary>
    /// Equality comparison override in case of hash collision
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is ScrapeDefinitionBatchProperties))
            return false;

        MyClass other = (ScrapeDefinitionBatchProperties)obj;
        return this.ResourceType == other.ResourceType && this.AzureMetricConfiguration.ToUniqueStringRepresentation() == other.ToUniqueStringRepresentation() && this.SubscriptionId == other.SubscriptionId && this.AggregationInterval.Equals(other.AggregationDeserializer);
    }
}