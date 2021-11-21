namespace Promitor.Core.Scraping.Configuration.Model.Metrics
{
    // TODO: Unit tests
    public class MetricInformation
    {
        /// <summary>
        ///     Name of the Azure Monitor metric to query
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Name of the namespace containing the metric
        /// </summary>
        public string Namespace { get; set; }
    }
}
