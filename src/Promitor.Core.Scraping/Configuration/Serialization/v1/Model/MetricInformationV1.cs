namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class MetricInformationV1
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
