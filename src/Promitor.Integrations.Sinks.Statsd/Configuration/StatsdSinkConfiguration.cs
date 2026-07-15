namespace Promitor.Integrations.Sinks.Statsd.Configuration
{
    public class StatsdSinkConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 8125;
        public string MetricPrefix { get; set; }
        public StatsdFormatterTypesEnum MetricFormat { get; set; } = StatsdFormatterTypesEnum.Default;
        public GenevaConfiguration Geneva { get; set; }

        /// <summary>
        ///     When <c>true</c>, a scraped measurement that has no value (<c>null</c>, e.g. because Azure Monitor
        ///     emitted no datapoint for the period) is not written to StatsD, so downstream systems observe a genuine
        ///     data gap instead of a fabricated value. When <c>false</c> (default), the historical behavior is kept and
        ///     a missing value is reported as <c>0</c>.
        ///     This is the StatsD counterpart of the Prometheus sink's <c>metricUnavailableValue</c> (which defaults to
        ///     NaN); StatsD gauges cannot represent NaN, so absence is expressed by not emitting the sample.
        /// </summary>
        public bool DropMetricsWithNoValue { get; set; } = false;
    }
}