using Promitor.Core.Contracts;

namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    /// <summary>
    /// Defines properties of a batch of scrape definitions 
    /// </summary>
    public class ScrapeDefinitionBatchProperties 
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
    /// Builds a namespaced string key to satisfy batch restrictions 
    /// </summary>
    private string BuildBatchHashKey()
    {
        return "";
    }
}