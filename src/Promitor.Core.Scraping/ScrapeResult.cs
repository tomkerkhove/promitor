using GuardNet;

namespace Promitor.Core.Scraping
{
    public class ScrapeResult
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="resourceUri">Uri of the resource that was scraped</param>
        /// <param name="metricValue">Value of the metric that was found</param>
        public ScrapeResult(string resourceUri, double metricValue)
        {
            Guard.NotNullOrEmpty(resourceUri, nameof(resourceUri));

            ResourceUri = resourceUri;
            MetricValue = metricValue;
        }

        /// <summary>
        ///     Uri of the resource that was scraped
        /// </summary>
        public string ResourceUri { get; }

        /// <summary>
        ///     Value of the metric that was found
        /// </summary>
        public double MetricValue { get; }
    }
}