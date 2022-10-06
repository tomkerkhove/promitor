namespace Promitor.Core.Scraping.Configuration.Model
{
    public class LogAnalyticsConfiguration
    {
        /// <summary>
        ///     Configuration on how to query the log analytics
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        ///     Configuration on how to aggregate the query
        /// </summary>
        public Aggregation Aggregation { get; set; }
    }
}